function CategoryViewModel() {
    var self = this;
    self.isBound = false;

    self.categories = ko.observableArray(null);
    self.categoryName = ko.observable();
    self.isRequestInProgress = ko.observable(false);
    self.currentCategory = ko.observable({ Name: '' });

    self.loadCategories = function () {
        self.isRequestInProgress(true);

        $.get(pageCategoryUrls.loadUrl, null, function (data) {
            self.categories(data);
            self.isRequestInProgress(false);
        });
    }

    self.edit = function (data) {
        self.currentCategory(data);
    }

    self.save = function (data) {
        if (self.currentCategory() == null || self.currentCategory().Name == null || self.currentCategory().Name.length <= 0)
        {
            alert('Please enter a category name.');
            return;
        }

        self.isRequestInProgress(true);

        $.post(pageCategoryUrls.saveUrl, self.currentCategory(),
        function (data) {
            self.loadCategories();
            self.isRequestInProgress(false);
            self.currentCategory({ Name: '' });
        });
    }

    self.remove = function (data) {
        self.isRequestInProgress(true);
        $.post(pageCategoryUrls.removeUrl, { id: data.Id }, function (res) {
            self.isRequestInProgress(false);

            if (res === true)
                self.loadCategories();
            else if(res>0)
            {
                var deleteConfirm = confirm('There are ' + res + ' page(s) in this category. Force delete category?');

                if (deleteConfirm) {
                    self.isRequestInProgress(true);
                    $.post(pageCategoryUrls.removeUrl, { id: data.Id,force:true }, function (res2) {
                        self.isRequestInProgress(false);
                        self.loadCategories();
                    });
                }
            }
        });
    }
}

var categoryVm = new CategoryViewModel();
function bindCategoryViewModel() {

    if(!categoryVm.isBound)
        ko.applyBindings(categoryVm, $('#categoryManagerContainer')[0]);

    categoryVm.isBound = true;
    categoryVm.loadCategories();
}