using AvaloniaUI.MCP.Tools;

namespace AvaloniaUI.MCP.Tests;

[TestClass]
public class XamlValidationDebugTest
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public void Debug_XamlValidation_Result()
    {
        // Arrange
        string validXaml = @"<Window xmlns=""https://github.com/avaloniaui""
                                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                             <TextBlock Text=""Hello World"" />
                          </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(validXaml);

        // Assert
        TestContext.WriteLine($"Validation result: {result}");

        // Just verify it doesn't crash
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0, "Result should not be empty");
    }

    [TestMethod]
    public void DockPanel_ShouldNotWarn()
    {
        // Arrange - DockPanel is available in AvaloniaUI and should NOT trigger a warning
        string xamlWithDockPanel = @"<Window xmlns=""https://github.com/avaloniaui""
                                         xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                                     <DockPanel>
                                         <TextBlock Text=""Test"" />
                                     </DockPanel>
                                  </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(xamlWithDockPanel);

        // Assert
        TestContext.WriteLine($"Validation result: {result}");
        Assert.IsFalse(result.Contains("DockPanel is not available"),
            "DockPanel should not trigger WPF-only warning as it's available in AvaloniaUI");
    }

    [TestMethod]
    public void UniformGrid_ShouldNotWarn()
    {
        // Arrange - UniformGrid is available in AvaloniaUI and should NOT trigger a warning
        string xamlWithUniformGrid = @"<Window xmlns=""https://github.com/avaloniaui""
                                           xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                                       <UniformGrid Rows=""2"" Columns=""2"">
                                           <TextBlock Text=""Test"" />
                                       </UniformGrid>
                                    </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(xamlWithUniformGrid);

        // Assert
        TestContext.WriteLine($"Validation result: {result}");
        Assert.IsFalse(result.Contains("UniformGrid is not available"),
            "UniformGrid should not trigger WPF-only warning as it's available in AvaloniaUI");
    }

    [TestMethod]
    public void Viewbox_ShouldNotWarn()
    {
        // Arrange - Viewbox is available in AvaloniaUI and should NOT trigger a warning
        string xamlWithViewbox = @"<Window xmlns=""https://github.com/avaloniaui""
                                       xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                                   <Viewbox>
                                       <TextBlock Text=""Test"" />
                                   </Viewbox>
                                </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(xamlWithViewbox);

        // Assert
        TestContext.WriteLine($"Validation result: {result}");
        Assert.IsFalse(result.Contains("Viewbox is not available"),
            "Viewbox should not trigger WPF-only warning as it's available in AvaloniaUI");
    }

    [TestMethod]
    public void FlowDocument_ShouldWarn()
    {
        // Arrange - FlowDocument is WPF-only and should trigger a warning
        string xamlWithFlowDocument = @"<Window xmlns=""https://github.com/avaloniaui""
                                            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                                        <FlowDocument>
                                            <Paragraph>Test</Paragraph>
                                        </FlowDocument>
                                     </Window>";

        // Act
        string result = XamlValidationTool.ValidateXaml(xamlWithFlowDocument);

        // Assert
        TestContext.WriteLine($"Validation result: {result}");
        Assert.IsTrue(result.Contains("FlowDocument is not available"),
            "FlowDocument should trigger WPF-only warning as it's not available in AvaloniaUI");
    }

    [TestMethod]
    public void WpfConversion_FindAncestor_ConvertsToParentSyntax()
    {
        // Arrange - WPF FindAncestor RelativeSource binding
        string wpfXaml = @"<Window xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                               xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                           <TextBlock Text=""{Binding RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type Window}}, Path=Title}"" />
                        </Window>";

        // Act
        string result = XamlValidationTool.ConvertWpfXamlToAvalonia(wpfXaml);

        // Assert
        TestContext.WriteLine($"Conversion result: {result}");
        Assert.IsTrue(result.Contains("$parent[Window]"),
            "FindAncestor binding should be converted to $parent syntax");
        Assert.IsTrue(result.Contains("Applied") && result.Contains("automatic conversion"),
            "Conversion summary should report automatic conversions");
    }

    [TestMethod]
    public void WpfConversion_NamespaceReplacement_Works()
    {
        // Arrange - WPF presentation namespace
        string wpfXaml = @"<Window xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                               xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                           <Button Content=""Test"" />
                        </Window>";

        // Act
        string result = XamlValidationTool.ConvertWpfXamlToAvalonia(wpfXaml);

        // Assert
        TestContext.WriteLine($"Conversion result: {result}");
        Assert.IsTrue(result.Contains("https://github.com/avaloniaui"),
            "WPF namespace should be replaced with AvaloniaUI namespace");
        Assert.IsTrue(result.Contains("Replaced WPF presentation namespace"),
            "Conversion notes should confirm namespace replacement");
    }

    [TestMethod]
    public void WpfConversion_DetectsDependencyProperty()
    {
        // Arrange - XAML with DependencyProperty usage
        string wpfXaml = @"<Window xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                               xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                           <!-- DependencyProperty should be converted to AvaloniaProperty -->
                           <TextBlock Text=""Test"" />
                        </Window>";

        // Act
        string result = XamlValidationTool.ConvertWpfXamlToAvalonia(wpfXaml);

        // Assert
        TestContext.WriteLine($"Conversion result: {result}");
        Assert.IsTrue(result.Contains("DependencyProperty") && result.Contains("AvaloniaProperty"),
            "Should detect DependencyProperty and suggest AvaloniaProperty conversion");
        Assert.IsTrue(result.Contains("docs.avaloniaui.net"),
            "Should include link to documentation");
    }

    [TestMethod]
    public void WpfConversion_DetectsTriggers()
    {
        // Arrange - XAML with Trigger (WPF-specific)
        string wpfXaml = @"<Window xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                               xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                           <Button>
                               <Button.Triggers>
                                   <EventTrigger RoutedEvent=""Button.Click"">
                                   </EventTrigger>
                               </Button.Triggers>
                           </Button>
                        </Window>";

        // Act
        string result = XamlValidationTool.ConvertWpfXamlToAvalonia(wpfXaml);

        // Assert
        TestContext.WriteLine($"Conversion result: {result}");
        Assert.IsTrue(result.Contains("Trigger usage detected"),
            "Should detect Trigger usage");
        Assert.IsTrue(result.Contains("Pseudo-classes") || result.Contains(":pointerover"),
            "Should suggest AvaloniaUI alternatives like pseudo-classes");
    }

    [TestMethod]
    public void WpfConversion_EmptyXaml_ReturnsError()
    {
        // Arrange
        string emptyXaml = "";

        // Act
        string result = XamlValidationTool.ConvertWpfXamlToAvalonia(emptyXaml);

        // Assert
        Assert.IsTrue(result.Contains("Error") || result.Contains("cannot be empty"),
            "Should return error for empty XAML");
    }

    [TestMethod]
    public void WpfConversion_MultipleFinAncestors_AllConverted()
    {
        // Arrange - Multiple FindAncestor patterns
        string wpfXaml = @"<Window xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                               xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                           <Grid>
                               <TextBlock Text=""{Binding RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type Grid}}, Path=Name}"" />
                               <TextBlock Text=""{Binding RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type Window}}, Path=Title}"" />
                           </Grid>
                        </Window>";

        // Act
        string result = XamlValidationTool.ConvertWpfXamlToAvalonia(wpfXaml);

        // Assert
        TestContext.WriteLine($"Conversion result: {result}");
        Assert.IsTrue(result.Contains("$parent[Grid]"),
            "Should convert Grid FindAncestor");
        Assert.IsTrue(result.Contains("$parent[Window]"),
            "Should convert Window FindAncestor");
        Assert.IsTrue(result.Contains("2") && result.Contains("automatic conversion"),
            "Should report 2 automatic conversions");
    }
}