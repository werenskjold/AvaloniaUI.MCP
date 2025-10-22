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
}