var archiwum = archiwum || {};

archiwum.yearModel = Backbone.Model.extend({
    defaults: {
        Name: '',
        YearId: 0
    }
})
archiwum.yearsCollection = Backbone.Collection.extend({
    model: archiwum.yearModel
});

archiwum.graduateModel = Backbone.Model.extend({
    defaults: {
        FirstName: '',
        LastName: ''
    }
})
archiwum.graduatesCollection = Backbone.Collection.extend({
    model: archiwum.graduateModel
});

archiwum.appModel = Backbone.Model.extend({
    defaults: {
        Years: new archiwum.yearsCollection(),
        IDY: 0
    },
    urlRoot: '/Home/AppModel'
})





archiwum.yearView = Backbone.View.extend({
    events: {
        'click div.year': 'yearClick'
    },
    initialize: function () {
        this.template = _.template($('#year-view-template').html());
    },
    yearClick: function (e) {
        Backbone.trigger('graduatesLoad', {IDY: this.model.attributes.YearId});
    },
    render: function () {
        this.$el.html(this.template(this.model.toJSON()));
        return this;
    }
})

archiwum.graduateView = Backbone.View.extend({
    events: {
        'click div.year': 'yearClick'
    },
    initialize: function () {
        this.template = _.template($('#graduate-view-template').html());
    },
    yearClick: function (e) {
        Backbone.trigger('graduatesLoad', { IDY: this.model.attributes.YearId });
    },
    render: function () {
        this.$el.html(this.template(this.model.toJSON()));
        return this;
    }
})

archiwum.app = Backbone.View.extend({
    el: $('#archiwum-app'),
    initialize: function (options) {
        var self = this;
        this.options = options;
        this.model = new archiwum.appModel();
        Backbone.on('graduatesLoad',function(data){
            var IDY = data.IDY;
            self.graduatesLoad(IDY);
        });
        //this.model.set('IDY', options.IDY);
        this.yearsLoad();
    },
    yearsLoad: function () {
        var self = this;
        self.model.fetch({
            data: {
                IDY: self.model.get('IDY')
            }
        }).success(function () {
            var yearsCollection = new archiwum.yearsCollection(self.model.get('Years'));
            yearsCollection.each(function (year) {
                var view = new archiwum.yearView({ model: year });
                self.$el.find('div.years').append(view.render().el);
            });
        }).error(function(response){
            $.PublishEvents(response.responseJSON);
        }) 
    },
    graduatesLoad: function (IDY) {
        var self = this;
        var idy = IDY;
        console.log(idy)
        self.model.fetch({
            data: {
                IDY: self.model.get('IDY')
            }
        }).success(function () {
            var yearsCollection = new archiwum.yearsCollection(self.model.get('Years'));
            yearsCollection.each(function (year) {
                var view = new archiwum.yearView({ model: year });
                self.$el.find('div.years').append(view.render().el);
            });
            console.log('get')
            $.get('/Home/Graduates', { YearId: idy }, function (data) {

                var graduatesCollection = new archiwum.graduatesCollection(data.graduates);
                console.log(graduatesCollection)
                graduatesCollection.each(function (graduate) {
                    var view = new archiwum.graduateView({ model: graduate });
                    self.$el.find('div.graduates').append(view.render().el);
                })
            })
        }).error(function(response){
            $.PublishEvents(response.responseJSON);
        }) 
    }
})