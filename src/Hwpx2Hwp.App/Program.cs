namespace Hwpx2Hwp.App;

/// <summary>
/// 진입점(1단계 뼈대): 빈 창 하나만 띄운다. 엔진·BatchRunner 조립은 이후 단계에서 붙인다(docs/design/baseline.md D-8).
/// </summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var form = new Form { Text = "hwpx2hwp" };
        Application.Run(form);
    }
}
