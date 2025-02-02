var config = require('./config');
var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
var mongoose = require('mongoose');
var mongoServer=require('mongodb-memory-server');
var ordersRouter = require('./routes/orders');
var factoriesRouter=require('./routes/factories');
var citiesRouter=require('./routes/cities');

var app = express();


// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
var port = config.APP_PORT || 4000;
app.listen(port);
console.log('App listening on port ' + port);
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));
app.use('/myco/api', ordersRouter);
app.use('/myco/api',factoriesRouter)
app.use('/myco/api',citiesRouter)

app.use(function (req, res, next) {
  // Website you wish to allow to connect
  res.setHeader('Access-Control-Allow-Origin', 'http://localhost:' + port)

  // Request methods you wish to allow
  res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE')

  // Request headers you wish to allow
  res.setHeader('Access-Control-Allow-Headers', 'Origin,X-Requested-With,Content-Type,Accept')

  // Pass to next layer of middleware
  next()
})

const mongoDBServer = new mongoServer.default();

mongoDBServer.getConnectionString().then((mongoURI)=>{
    mongoose.connect(mongoURI, { useNewUrlParser: true }); //Open connection to MongoDB
});

// catch 404 and forward to error handler
app.use(function (req, res, next) {
  next(createError(404));
});

// error handler
app.use(function (err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render('error');
});

module.exports = app;
