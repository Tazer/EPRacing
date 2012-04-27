
var viewModel = {};
var basket = function () {
    var pub = {};
    pub.init = function() {
        $.getJSON("/Basket/Get", function(data) {
            viewModel = ko.mapping.fromJS(data.Basket);
            ko.applyBindings(viewModel);
        });

    };

    pub.get = function () {
        $.getJSON("/Basket/Get", function (data) {
            ko.mapping.fromJS(data.Basket, viewModel);
        });
    };

    pub.delete = function () {
        console.log(this);
        console.log(this.Id());
        $.ajax({
            type: "POST",
            url: "/Basket/Delete",
            // The key needs to match your method's input parameter (case-sensitive).
            data: JSON.stringify({ id: this.Id() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                ko.mapping.fromJS(data.Basket, viewModel);
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    };


    pub.add = function (id) {
        $.ajax({
            type: "POST",
            url: "/Basket/Add",
            // The key needs to match your method's input parameter (case-sensitive).
            data: JSON.stringify({ id: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('.alertAdd').clearQueue().hide().css('opacity', '1').show().fadeTo(2000, 0, function () {
                    $('.alertAdd').hide().css('opacity', '1');
                });
                ko.mapping.fromJS(data.Basket, viewModel);
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    };
    return pub;
}();

$(document).ready(function () {

    basket.init();
    var $container = $('.thumbnails');

    $container.imagesLoaded(function () {
        $container.masonry({
            itemSelector: 'li.span3'
        });
    });

    $('#paymentForm').validate({
        rules: {
            email: {required: true , email : true}
        },
        messages: {
            email: 'Fyll i email'
        }
    });

    $('#modal,#modalCart').modal({
        keyboard: false
    });

    $('#modal,#modalCart').modal('hide');

    $('.shopBtn').click(function () {
        $('#modalCart').modal('show');
        return false;
    });

    $('.details').click(function () {
        $.get($(this).attr('rel'), function (data) {
            $('#modal .modal').html(data);
            $('#modal').modal('show');
        });
        return false;
    });
    $('.addto').live('click', function () {
        var id = parseInt($(this).attr('rel'));
        basket.add(id);
    });

    $('#goTop').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });

});