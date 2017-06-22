(function ($) {
    function Book() {
        var $this = this;

        function initilizeModel() {           
            $('#modal-action-book').on('show.bs.modal', function (event) {
                var button = $(event.relatedTarget); // Button that triggered the modal
                var url = button.attr("href");
                var modal = $(this);
                // note that this will replace the content of modal-content ever time the modal is opened
                modal.find('.modal-content').load(url);
            });


        }       
        $this.init = function () {
            
            initilizeModel();
        }
    }
    $(function () {
        var self = new Book();
        self.init();        
    })
}(jQuery))
