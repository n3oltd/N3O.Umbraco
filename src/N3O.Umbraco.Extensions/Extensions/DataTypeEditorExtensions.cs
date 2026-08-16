using N3O.Umbraco.DataTypes;

namespace N3O.Umbraco.Extensions;

public static class DataTypeEditorExtensions {
    public static BlockGridDataTypeDesigner NewBlockGrid(this IDataTypeEditor editor, string name) {
        return editor.New<BlockGridDataTypeDesigner>(name);
    }

    public static BlockListDataTypeDesigner NewBlockList(this IDataTypeEditor editor, string name) {
        return editor.New<BlockListDataTypeDesigner>(name);
    }

    public static CheckBoxListDataTypeDesigner NewCheckBoxList(this IDataTypeEditor editor, string name) {
        return editor.New<CheckBoxListDataTypeDesigner>(name);
    }

    public static ColorPickerDataTypeDesigner NewColorPicker(this IDataTypeEditor editor, string name) {
        return editor.New<ColorPickerDataTypeDesigner>(name);
    }

    public static ContentPickerDataTypeDesigner NewContentPicker(this IDataTypeEditor editor, string name) {
        return editor.New<ContentPickerDataTypeDesigner>(name);
    }

    public static DateTimeDataTypeDesigner NewDateTime(this IDataTypeEditor editor, string name) {
        return editor.New<DateTimeDataTypeDesigner>(name);
    }

    public static DropdownDataTypeDesigner NewDropdown(this IDataTypeEditor editor, string name) {
        return editor.New<DropdownDataTypeDesigner>(name);
    }

    public static FileUploadDataTypeDesigner NewFileUpload(this IDataTypeEditor editor, string name) {
        return editor.New<FileUploadDataTypeDesigner>(name);
    }

    public static MarkdownDataTypeDesigner NewMarkdown(this IDataTypeEditor editor, string name) {
        return editor.New<MarkdownDataTypeDesigner>(name);
    }

    public static MediaPickerDataTypeDesigner NewMediaPicker(this IDataTypeEditor editor, string name) {
        return editor.New<MediaPickerDataTypeDesigner>(name);
    }

    public static MultiNodeTreePickerDataTypeDesigner NewMultiNodeTreePicker(this IDataTypeEditor editor,
                                                                             string name) {
        return editor.New<MultiNodeTreePickerDataTypeDesigner>(name);
    }

    public static MultipleTextStringDataTypeDesigner NewMultipleTextString(this IDataTypeEditor editor, string name) {
        return editor.New<MultipleTextStringDataTypeDesigner>(name);
    }

    public static MultiUrlPickerDataTypeDesigner NewMultiUrlPicker(this IDataTypeEditor editor, string name) {
        return editor.New<MultiUrlPickerDataTypeDesigner>(name);
    }

    public static RadioButtonListDataTypeDesigner NewRadioButtonList(this IDataTypeEditor editor, string name) {
        return editor.New<RadioButtonListDataTypeDesigner>(name);
    }

    public static SliderDataTypeDesigner NewSlider(this IDataTypeEditor editor, string name) {
        return editor.New<SliderDataTypeDesigner>(name);
    }

    public static TagsDataTypeDesigner NewTags(this IDataTypeEditor editor, string name) {
        return editor.New<TagsDataTypeDesigner>(name);
    }

    public static TextareaDataTypeDesigner NewTextarea(this IDataTypeEditor editor, string name) {
        return editor.New<TextareaDataTypeDesigner>(name);
    }

    public static TextBoxDataTypeDesigner NewTextBox(this IDataTypeEditor editor, string name) {
        return editor.New<TextBoxDataTypeDesigner>(name);
    }

    public static ToggleDataTypeDesigner NewToggle(this IDataTypeEditor editor, string name) {
        return editor.New<ToggleDataTypeDesigner>(name);
    }
}
