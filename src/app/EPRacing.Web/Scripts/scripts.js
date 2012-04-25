$(document).ready(function(){  
    
    var $container = $('.thumbnails');

    $container.imagesLoaded( function(){
        $container.masonry({
            itemSelector : 'li.span3'
        });
    });

	
    $('#modal,#modalCart').modal({
        keyboard: false
    });
    
    $('#modal,#modalCart').modal('hide');
    
    $('.shopBtn').click(function(){
        $('#modalCart').modal('show');
        return false;
    });
    
    $('.details').click(function(){
        $.get($(this).attr('rel'), function(data) {
            $('#modal .modal').html(data);
            $('#modal').modal('show');
        });
        return false;
    });
    
    $('.addto').live('click',function(event){
        $('.buyNow').show();
        var itemsNb = parseInt ($('.shopBtn span').html());
        itemsNb++;
        var id = parseInt($(this).attr('rel'));
        $.getJSON('json/items.json', function(data) {
            trash = '<a href="#" class="trash"><i class="icon-trash"></i></a>';
            $('.cartTab tbody').append('<tr><td>'+data.items[id-1].name+'</td><td class="tdPrice">$ <span>'+data.items[id-1].price+'</span></td><td>'+trash+'</td></tr>');
            $('#amount').html(parseInt($('#amount').html())+parseInt(data.items[id-1].price));
        });
        $('.shopBtn span').html(itemsNb);
        $('.alertAdd').clearQueue().hide().css('opacity','1').show().fadeTo(2000,0,function(){
            $('.alertAdd').hide().css('opacity','1');
        });
        return false;
    });
    
    $('.trash').live('click',function(){
        var price = parseInt($(this).parent().prev().children('span').html());
        $('#amount').html(parseInt($('#amount').html())- price);
        $(this).parent().parent().remove();
        $('.shopBtn span').html(parseInt($('.shopBtn span').html()) -1 );
        if ($('#amount').html()==0) $('.buyNow').hide();
        return false;
        
    });
    
    
    $('#goTop').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });

});