
// MobileCompat 使用示例
(function(){
	if (typeof MobileCompat === 'undefined') return; // ensure script loaded

	var mc = new MobileCompat({
		disableUserZoom: true,
		disableDoubleTap: true,
		addResizeHandler: true,
		onResize: function(){ console.log('window resized/orientation changed'); }
	}).init();

	if (mc.isMobile()) console.log('Mobile device detected');

	// 示例：标准化 touch 事件位置
	document.addEventListener('touchstart', function(e){
		var t = mc.normalizeTouch(e);
		// do something with t.x / t.y
	}, mc._passiveSupported ? {passive:true} : false);
})();
//测试
