namespace ParkShareIdentity.Enums
{
    public enum SpaceAvailability
    {
        AlwaysOnRent =0,
       NotForRentAtTheMoment=1, 
       RentableAtCertainTime=2
    }
    public enum PreviewTimeInWeeks
    {
        One = 1,
        Two = 2,
        Three = 3
    }
    public enum Days
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
    public enum TimeOrDays
    {
        Days=0,
        Time=1
    }
}
