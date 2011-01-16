using System;
using System.Reflection;

namespace Validate
{
    public class TargetMemberMetadata
    {
        public Type Type { get; private set; }
        public MemberInfo Member { get; set; }
        public string MemberName { get { return Member == null ? "{{ Target member could not be determined }}" : Member.Name; } }
        public string Path { get; set; }

        public TargetMemberMetadata(Type type, MemberInfo member, string path)
        {
            Type = type;
            Member = member;
            Path = path;
        }
    }
}