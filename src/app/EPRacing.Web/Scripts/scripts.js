
var viewModel = {};
var productModel = {};

var product = function () {
    var priv = {};
    priv.inited = false;
    var pub = { };
    pub.showDetails = function(id) {
        $.getJSON("/Product/Index/" + id, function (data) {
  
            
            if(priv.inited)
                ko.mapping.fromJS(data, productModel);
            else {
                productModel = ko.mapping.fromJS(data);
                ko.applyBindings(productModel,document.getElementById("modalProductDetail"));
                priv.inited = true;
            }
            $('#modalProductDetail').modal('show');
        });
    };
    return pub;
}();

var basket = function () {
    var pub = {};
    pub.init = function() {
        $.getJSON("/Basket/Get", function(data) {
            viewModel = ko.mapping.fromJS(data.Basket);
            ko.applyBindings(viewModel, document.getElementById("modalCart"));
            ko.applyBindings(viewModel, document.getElementById("header"));
        });

    };

    pub.get = function () {
        $.getJSON("/Basket/Get", function (data) {
            ko.mapping.fromJS(data.Basket, viewModel);
        });
    };

    pub.delete = function () {
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
    pub.addFromDataBind = function () {
        pub.add(this.Id());
    };

    pub.add = function (id) {
        $('#modalProductDetail').modal('hide');
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

    $('#modal,#modalCart,#modalProductDetail').modal({
        keyboard: false
    });

    $('#modal,#modalCart,#modalProductDetail').modal('hide');

    $('.shopBtn').click(function () {
        $('#modalCart').modal('show');
        return false;
    });

    $('.details').click(function () {
        product.showDetails($(this).attr('rel'));
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