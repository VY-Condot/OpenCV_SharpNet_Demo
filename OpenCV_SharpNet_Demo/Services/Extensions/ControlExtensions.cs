// Helper class to enable Double Buffering on any Control
public static class ControlExtensions
{
    public static void DoubleBuffered(this Control control, bool enable)
    {
        var propertyInfo = typeof(Control).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        propertyInfo?.SetValue(control, enable, null);
    }
}