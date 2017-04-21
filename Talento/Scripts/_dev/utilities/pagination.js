var Pagination = function ($parent, $innerChild) {

    this.parent = $parent;
    this.innerChild = $innerChild;

    // Global This accesible
    var self = this;

    this.watch = function () {
        self.innerChild.on("click", self.parent, function (event) {
            event.preventDefault();
            // Get Child
            var $child = event.target;
            // Target Element A
            if ('A' == $child.tagName) {
                var $url = $child.getAttribute("href");
                if ("#" !== $url) {
                    var $typeClass = "slide-right";
                    var $parent = $(event.target.parentElement);
                    // Animation Left or Right
                    if ($parent.hasClass("pagination-arrow")) {
                        if ($parent.hasClass("pagination-prev")) {
                            $typeClass = "slide-left";
                        }
                    } else {
                        var nextPage = $child.innerHTML;
                        var currentPage = $("#pagination-log").find(".active a")[0].innerHTML;
                        if (nextPage < currentPage) {
                            $typeClass = "slide-left";
                        }
                    }
                    self.ajaxCall($url, $typeClass);
                }
            }
        });
    }

    // Ajax call
    this.ajaxCall = function ($href, $class) {
        document.body.style.cursor = 'wait';
        $.ajax({
            url: $href,
            data: { clase: $class }

        }).done(function (pagination) {
            self.innerChild.html(pagination);
            document.body.style.cursor = 'default';
        });
    }

    return {
        Create: function () {
            self.watch();
        },
    };
}
// Export Pagination
module.exports = Pagination;
