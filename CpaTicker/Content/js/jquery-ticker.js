(function ($) {

    $.fn.ticker = function (options) {

        var settings = $.extend({
            // These are the defaults.
            duration: 15000,
            direction: true
           
        }, options);

        return this.bind('sticker', function () 
        {
            var $this = $(this),
                width = $this.width();

            //$this.animate_ticker(settings);

            if (settings.direction) {
                $this.css({ right: -width });
            }
            else {
                $this.css({ right: $this.parent().width() }); // test
            }



            TAnimation(this, settings);
        }).trigger('sticker');

    }

    function TAnimation(element, options) {
       // debugger;
        //var $this = this.$element;
        //var options = this.options;

        var $this = $(element),
            width = $this.width(),
            offset = $this.parent().width(),

            animateto = options.direction ? offset : -width,



            //duration = options.duration, //15000,

            duration = ((width + offset) * options.duration) / 1000;
           


        //return $this.animate({ right: offset }, duration * position / (offset + width), 'linear', function () {
        //    $this.trigger('sticker');

        return $this.animate({ right: animateto }, duration, 'linear', function () {
            $this.trigger('sticker');
        });
    }

    //$.fn.animate_ticker = function (options) {

    //    var $this = $(this),
    //        width = $this.width(),
    //        offset = $this.parent().width(),
    //        duration = options.duration,//15000,
    //        position = offset - parseInt($this.css('right'));

    //    return this.animate({ right: offset }, duration * position / (offset + width), 'linear', function () {
    //        $this.trigger('sticker');
    //    });

    //}

   

}(jQuery));