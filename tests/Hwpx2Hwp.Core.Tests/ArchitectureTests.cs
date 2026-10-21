using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Hwpx2Hwp.Core.Tests;

/// <summary>
/// 구조 규칙 검사(docs/design/baseline.md D-10 2). 소스 폴더를 읽어 검사하므로
/// 아직 없는 프로젝트(HwpCom·App)는 검사 대상 파일이 없을 뿐 통과한다.
/// </summary>
public sealed class ArchitectureTests
{
    private const string SolutionFileName = "Hwpx2Hwp.sln";
    private const string HwpComFolder = "src/Hwpx2Hwp.HwpCom/";
    private const string AppFolder = "src/Hwpx2Hwp.App/";

    private static readonly string[] ScannedRoots = ["src", "tests"];

    // ArchitectureTests 자신은 금지 문자열을 패턴으로 담고 있으므로 검사에서 뺀다.
    private static readonly string SelfRelativePath = "tests/Hwpx2Hwp.Core.Tests/ArchitectureTests.cs";

    private static readonly Regex ComOrRegistryPattern = new(
        @"\bdynamic\b|HWPFrame|Microsoft\.Win32\.Registry|\bRegistryKey\b|\bRegistry\.",
        RegexOptions.CultureInvariant);

    private static readonly Regex WinFormsPattern = new(
        @"System\.Windows\.Forms|<UseWindowsForms>\s*true\s*</UseWindowsForms>",
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    [Fact]
    public void D10_2_1_Core_csproj에_ProjectReference_PackageReference_없음()
    {
        var root = FindRepoRoot();
        var csprojPath = Path.Combine(root, "src", "Hwpx2Hwp.Core", "Hwpx2Hwp.Core.csproj");
        Assert.True(File.Exists(csprojPath), $"Core 프로젝트 파일이 없다: {csprojPath}");

        var references = XDocument.Load(csprojPath)
            .Descendants()
            .Where(e => e.Name.LocalName is "ProjectReference" or "PackageReference")
            .Select(e => $"{e.Name.LocalName} {(string?)e.Attribute("Include")}")
            .ToList();

        Assert.True(references.Count == 0, "Core 는 참조 0 이어야 한다: " + string.Join(", ", references));
    }

    [Fact]
    public void D10_2_2_dynamic_HWPFrame_Registry_는_HwpCom_밖에_없음()
    {
        var violations = FindViolations(ComOrRegistryPattern, HwpComFolder, "*.cs");

        Assert.True(violations.Count == 0, "HwpCom 밖에서 COM·레지스트리 사용: " + string.Join("; ", violations));
    }

    [Fact]
    public void D10_2_3_WinForms_는_App_밖에_없음()
    {
        var violations = FindViolations(WinFormsPattern, AppFolder, "*.cs")
            .Concat(FindViolations(WinFormsPattern, AppFolder, "*.csproj"))
            .ToList();

        Assert.True(violations.Count == 0, "App 밖에서 WinForms 사용: " + string.Join("; ", violations));
    }

    private static List<string> FindViolations(Regex pattern, string allowedFolder, string searchPattern)
    {
        var root = FindRepoRoot();
        var violations = new List<string>();

        foreach (var file in EnumerateSourceFiles(root, searchPattern))
        {
            var relative = ToRelative(root, file);
            if (relative.StartsWith(allowedFolder, StringComparison.OrdinalIgnoreCase)
                || string.Equals(relative, SelfRelativePath, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var lines = File.ReadAllLines(file);
            for (var i = 0; i < lines.Length; i++)
            {
                var match = pattern.Match(lines[i]);
                if (match.Success)
                {
                    violations.Add($"{relative}:{i + 1} '{match.Value}'");
                }
            }
        }

        return violations;
    }

    private static IEnumerable<string> EnumerateSourceFiles(string root, string searchPattern)
    {
        foreach (var scanned in ScannedRoots)
        {
            var dir = Path.Combine(root, scanned);
            if (!Directory.Exists(dir))
            {
                continue;
            }

            foreach (var file in Directory.EnumerateFiles(dir, searchPattern, SearchOption.AllDirectories))
            {
                if (!IsBuildOutput(ToRelative(root, file)))
                {
                    yield return file;
                }
            }
        }
    }

    private static bool IsBuildOutput(string relativePath)
    {
        var segments = relativePath.Split('/');
        return segments.Any(s => s is "bin" or "obj");
    }

    private static string ToRelative(string root, string path) =>
        Path.GetRelativePath(root, path).Replace('\\', '/');

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, SolutionFileName)))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            $"{SolutionFileName} 이 있는 상위 폴더를 찾지 못했다: start={AppContext.BaseDirectory}");
    }
}
