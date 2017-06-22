var BookClientManager = (function($){
    'use strict';
    var filterApiUrl = null;

    function enableDisableFilterDropdown($fsearch, enable) {
        var $fvGroup = $('#filter-value-group');
        if (enable) {
            $fsearch.prop('disabled', false);
            $fvGroup.removeClass('dim-filter-value');
        } else {
            $fsearch.prop('disabled', true);
            $fvGroup.addClass('dim-filter-value');
        }
    }
    function loadFilterValueDropdown(filterBy, filterValue) {        
        filterValue = filterValue || '';
        var $fsearch = $('#filter-value-dropdown');
        enableDisableFilterDropdown($fsearch, false);
        if (filterBy !== 'NoFilter') {
            //it is a proper filter val, so get the filter            
            $.ajax({
                url: filterApiUrl,
                data: { FilterBy: filterBy }
            })
                .done(function (indentAndResult) {
                    //This removes the existing dropdownlist options
                    $fsearch
                        .find('option')
                        .remove()
                        .end()
                        .append($('<option></option>')
                            .attr('value', '')
                            .text('Select filter...'));

                    indentAndResult.forEach(function (arrayElem) {
                        $fsearch.append($("<option></option>")
                            .attr("value", arrayElem.value)
                            .text(arrayElem.text));
                    });
                    $fsearch.val(filterValue);
                    enableDisableFilterDropdown($fsearch, true);
                })
                .fail(function() {
                    alert("error");
                });
        }
    }

    function submitForm(inputElem) {        
        var form = $(inputElem).parents('form');
        form.submit();        
    }

    return {
        initialize: function(filterBy, filterValue, url) {            
            filterApiUrl = url;
            loadFilterValueDropdown(filterBy, filterValue);            
        },
        submitForm: function(filterElem) {
            submitForm(filterElem)
        },
        filterByHasChanged: function(filterElem) {            
            var filterByValue = $(filterElem).find(":selected").val();
            loadFilterValueDropdown(filterByValue);
            if (filterByValue === "0") {
                submitForm(filterElem);
            }
        },
        loadFilterValueDropdown: function (filterBy, filterValue) {
            loadFilterValueDropdown(filterBy, filterValue);
        },
        borrowBooks: function(element, books, url) {   
            // // console.log(books);
            // console.log(books.filter(book=>book.Checked));
                     
            $.ajax({
                url: url,
                method: "POST",
                cache: false,
                dataType: "json",
                data: {books: books},
                beforeSend: function(){
                    $(element)
                    .attr("disabled", "disabled")
                    .attr("value", "Processing...");
                    
                },
                success: function(response) {
                    if(response) window.location.href = "index";
                }
            })
        }
    };
})(window.jQuery)