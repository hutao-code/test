;(function(global){
  'use strict';
  function MobileCompat(options){
    this.options = options || {};
    this._lastTouch = 0;
    this._passiveSupported = false;
  }

  MobileCompat.prototype.init = function(){
    if (this.options.viewport !== false) this._ensureViewport();
    this._detectPassiveSupport();
    if (this.options.disableDoubleTap !== false) this._preventDoubleTapZoom();
    if (this.options.addResizeHandler) this._attachResizeHandler();
    return this;
  };

  MobileCompat.prototype._ensureViewport = function(){
    var content = 'width=device-width,initial-scale=1';
    if (this.options.disableUserZoom) content += ',maximum-scale=1,user-scalable=no';
    var meta = document.querySelector('meta[name=viewport]');
    if (meta) meta.setAttribute('content', content);
    else {
      meta = document.createElement('meta');
      meta.name = 'viewport';
      meta.content = content;
      var head = document.getElementsByTagName('head')[0] || document.documentElement;
      head.appendChild(meta);
    }
  };

  MobileCompat.prototype._detectPassiveSupport = function(){
    try {
      var opts = Object.defineProperty({}, 'passive', {
        get: function(){ this._passiveSupported = true; }.bind(this)
      });
      window.addEventListener('testPassive', null, opts);
      window.removeEventListener('testPassive', null, opts);
    } catch(e) { this._passiveSupported = false; }
  };

  MobileCompat.prototype._preventDoubleTapZoom = function(){
    var self = this;
    document.addEventListener('touchend', function(e){
      var now = Date.now();
      if (now - self._lastTouch <= 300){
        e.preventDefault();
      }
      self._lastTouch = now;
    }, this._passiveSupported ? {passive:false} : false);
  };

  MobileCompat.prototype._attachResizeHandler = function(){
    var self = this;
    var id;
    window.addEventListener('resize', function(){
      clearTimeout(id);
      id = setTimeout(function(){
        if (typeof self.options.onResize === 'function') self.options.onResize();
      }, 100);
    }, this._passiveSupported ? {passive:true} : false);
  };

  MobileCompat.prototype.normalizeTouch = function(e){
    var t = (e.changedTouches && e.changedTouches[0]) || (e.touches && e.touches[0]) || e;
    return {x: t.clientX, y: t.clientY, target: t.target || e.target, event: e};
  };

  MobileCompat.prototype.isMobile = function(){
    return /Mobi|Android|iPhone|iPad|iPod|IEMobile|WPDesktop/i.test(navigator.userAgent);
  };

  MobileCompat.prototype.requestAnimationFrame = (function(){
    return window.requestAnimationFrame ||
      window.webkitRequestAnimationFrame ||
      window.mozRequestAnimationFrame ||
      function(cb){ return window.setTimeout(cb, 16); };
  })();

  MobileCompat.prototype.cancelAnimationFrame = (function(){
    return window.cancelAnimationFrame ||
      window.webkitCancelAnimationFrame ||
      window.mozCancelAnimationFrame ||
      function(id){ clearTimeout(id); };
  })();

  global.MobileCompat = MobileCompat;
})(this);
