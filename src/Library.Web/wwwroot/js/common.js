var Common = (function ($) {
    'use strict';
    function UpdateLabel(labelContainer, label) {
        $(labelContainer).text(label);
    }
    function readURL(input, previewElement) {
        var $previewElement = previewElement;
        console.log($previewElement);
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = (e)=>{
                $($previewElement).attr('src', e.target.result);               
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    return {
        FileBrowsed: function (element, previewElement, labelContainer = null) {            
            var input = $(element),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            readURL(element, previewElement);
            labelContainer && UpdateLabel(labelContainer, label);
        }
    }
})(window.jQuery);