var Modal = (function ($) {
    function initilizeModel(elementId) {        
        $('#' + elementId).on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget); // Button that triggered the modal            
            var url = button.attr("href");            
            var modal = $(this);
            // note that this will replace the content of modal-content every time the modal is opened
            modal.find('.modal-content').load(url);
        });
    }
    return {
        init: function (elementId) {            
            initilizeModel(elementId);
        }
    };
})(window.jQuery)
