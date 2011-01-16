using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Validate.Extensions;

namespace Validate
{
    public class TargetMemberExpression<T>
    {
        private readonly LambdaExpression _expression;

        public TargetMemberExpression(LambdaExpression expression)
        {
            _expression = expression;
        }

        public bool Verify()
        {
            return IsMemberExpression() || IsMethodCallExpression() || IsParameterExpression();
        }

        private bool IsParameterExpression()
        {
            var pe = _expression.Body as ParameterExpression;
            return pe != null && pe.Type == typeof(T);
        }

        private bool IsMethodCallExpression()
        {
            var mce = _expression.Body as MethodCallExpression;
            return mce != null && mce.Object != null;
        }

        private bool IsMemberExpression()
        {
            return (_expression.Body is MemberExpression);
        }

        public TargetMemberMetadata GetTargetMemberMetadata()
        {
            if (IsParameterExpression())
            {
                return new TargetMemberMetadata(typeof(T), null, typeof(T).FriendlyName());
            }
            if (IsMemberExpression())
            {
                var me = (MemberExpression)_expression.Body;
                return new TargetMemberMetadata(me.Member.DeclaringType, me.Member, GetPath(me));
            }
            if (IsMethodCallExpression())
            {
                var mce = (MethodCallExpression)_expression.Body;
                return new TargetMemberMetadata(mce.Object.Type, mce.Method, GetPath(mce));
            }

            return new TargetMemberMetadata(typeof(T), null, GetPath(_expression.Body));
        }

        private string GetPath(Expression expression)
        {
            if (expression is MemberExpression)
            {
                var me = (MemberExpression)expression;
                return GetPath(me.Expression) + "." + me.Member.Name;
            }
            if (expression is MethodCallExpression)
            {
                var mce = (MethodCallExpression)expression;
                return GetPath(mce.Object) + "." + mce.Method.Name;
            }
            if (expression is ParameterExpression)
            {
                var pe = (ParameterExpression) expression;
                return pe.Type.Name;
            }
            return new Regex("\\w").Replace(expression.ToString(), typeof (T).Name, 1);
        }
    }
}