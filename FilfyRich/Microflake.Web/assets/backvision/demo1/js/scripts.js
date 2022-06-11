$(document).ready(function(){
	$('.header .menu a').on('click', function(e){
		$(this).parent().addClass('active').siblings().removeClass('active');
		var mobile_view = $('.header').hasClass('mobile-view');				
		var section = $('.container').find(this.hash);
		var offset = 45; if(!mobile_view) offset += $('.header').outerHeight();
		if(section.length) {
			$('body, html').animate({scrollTop: section.offset().top - offset}, 500, 'easeOutCubic');
		}
		e.preventDefault();
	});
	
	$(window).on('scroll', function(e){
		var top = $(window).scrollTop();
		var max_top = $('#video-header').height() - 30;
		var mobile_view = $('.header').hasClass('mobile-view');
		if(top > max_top && !mobile_view) {
			$('.header').addClass('header-fixed');
		} else {
			$('.header').removeClass('header-fixed');
		}
	}).trigger('scroll');
	
	
	$('.header .mobile').on('click', 'a', function(){
		var link = $(this); var header = $('.header'); var nav = header.find('nav');
		if(link.hasClass('mobile-menu') && !header.hasClass('menu-open')) {
			header.addClass('menu-open');
			nav.animate({height: nav.find('.menu').outerHeight()}, 500, 'easeOutCubic');
		} else {
			header.removeClass('menu-open');
			nav.animate({height: 0}, 300, 'easeOutCubic');
		}
		return false;
	});
	
	$(window).on('resize', function(){
		var header = $('.header');
		var is_mobile = header.find('.sizer').is(':visible');
		var has_class = header.hasClass('mobile-view');
		if(is_mobile && !has_class) header.addClass('mobile-view');
		if(!is_mobile && has_class) header.removeClass('mobile-view').removeClass('menu-open');
	}).triggerHandler('resize');

});