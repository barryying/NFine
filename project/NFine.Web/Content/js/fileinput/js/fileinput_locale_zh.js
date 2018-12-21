/*!
 * FileInput Spanish (Latin American) Translations
 *
 * This file must be loaded after 'fileinput.js'. Patterns in braces '{}', or
 * any HTML markup tags in the messages must not be converted or translated.
 *
 * @see http://github.com/kartik-v/bootstrap-fileinput
 *
 * NOTE: this file must be saved in UTF-8 encoding.
 */
(function ($) {
    "use strict";
    $.fn.fileinput.locales.zh = {
    		fileSingle: '单个文件',
            filePlural: '多个文件',
            browseLabel: '选择文件 &hellip;',
            removeLabel: '删除文件',
            removeTitle: '删除选中文件',
            cancelLabel: '取消',
            cancelTitle: '取消上传',
            uploadLabel: '上传',
            uploadTitle: '上传选中文件',
            msgSizeTooLarge: '文件 "{name}" (<b>{size} KB</b>) 超过允许上传的最大文件大小 <b>{maxSize} KB</b>. 请重新上传!',
            msgFilesTooLess: '文件数量必须大于 <b>{n}</b> {files} ，请重新上传！',
            msgFilesFull: '上传文件的最大数量为{m}!',
            msgFilesTooMany: '选择上传的文件数量 <b>({n})</b> 超过允许的最大数值 <b>{m}</b> ！ 请重新上传!',
            msgFileNotFound: '文件 "{name}" 未找到!',
            msgFileSecured: '安全限制阻止读取文件: "{name}".',
            msgFileNotReadable: '文件 "{name}" 不可读.',
            msgFilePreviewAborted: '文件预览异常： "{name}".',
            msgFilePreviewError: '读取文件时发生错误: "{name}".',
            msgInvalidFileType: '文件类型无效: "{name}". 只支持上传 "{types}" 这几种文件类型.',
            msgInvalidFileExtension: '文件扩展名无效: "{name}". 只支持上传 "{extensions}" 这几种文件扩展名.',
            msgValidationError: '文件上传失败',
            msgLoading: '正在加载 {index} / {files} &hellip;',
            msgProgress: '正在加载 {index} / {files} - {name} - 已完成 {percent}%.',
            msgSelected: '选中{n}个文件',
            msgFoldersNotAllowed: '只允许拖放文件! {n} 文件夹不允许拖放.',
            dropZoneTitle: '在此处拖放文件 &hellip;',
            indicatorNewTitle: '还未上传',
            confirmDelete: '注：您确定要【删除】该图片吗？删除不可恢复！',
    };

    $.extend($.fn.fileinput.defaults, $.fn.fileinput.locales.zh);
})(window.jQuery);
