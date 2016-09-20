namespace Alpha.Main.DataAccesses
{
    using Criteria.Password;
    using Models;

    public interface PasswordDataAccess
    {
        Result ResetMainAccountPassword(ResetMainAccountPasswordCriteria criteria);

        Result ChangeMainAccountPassword(UpdateMainAccountPasswordCriteria criteria);

        Result ResetSubAccountPassword(ResetSubAccountPasswordCriteria criteria);

        Result ChangeSubAccountPassword(UpdateSubAccountPasswordCriteria criteria);
    }
}