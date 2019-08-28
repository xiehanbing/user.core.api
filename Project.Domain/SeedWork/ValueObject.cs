namespace Project.Domain.SeedWork
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }

            return ReferenceEquals(left, null) || left.Equals(right);
        }


        protected static bool NotEqualOperatore(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }
    }
}