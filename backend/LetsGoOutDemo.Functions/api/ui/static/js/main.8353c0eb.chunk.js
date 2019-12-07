(window.webpackJsonp=window.webpackJsonp||[]).push([[0],{28:function(e,t,n){e.exports=n(62)},35:function(e,t,n){},37:function(e,t,n){},39:function(e,t,n){},57:function(e,t,n){},62:function(e,t,n){"use strict";n.r(t);var o=n(0),a=n(12),i=Boolean("localhost"===window.location.hostname||"[::1]"===window.location.hostname||window.location.hostname.match(/^127(?:\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$/));function r(e){navigator.serviceWorker.register(e).then(function(e){e.onupdatefound=function(){var t=e.installing;t&&(t.onstatechange=function(){"installed"===t.state&&(navigator.serviceWorker.controller?console.log("New content is available; please refresh."):console.log("Content is cached for offline use."))})}}).catch(function(e){console.error("Error during service worker registration:",e)})}n(33),n(35);var s,c=n(4),l=n(2),p=n(9),u=(n(37),function(e){function t(){return null!==e&&e.apply(this,arguments)||this}return c.c(t,e),t.prototype.render=function(){var e=this.props.state;return e.isLoggedIn?o.createElement("div",null,o.createElement("span",{className:"navbar-text ml-auto mr-3"},"Hello, ",e.nickName)):o.createElement("div",{className:"form-inline"},o.createElement("input",{className:"form-control mr-sm-2 nickname-input",type:"search",placeholder:"Your nickname...",value:e.nickNameInputText,onChange:this.onNickNameChanged,onKeyPress:this.onNickNameKeyPress}),o.createElement("button",{className:"btn btn-secondary login-button",type:"button",onClick:this.onLoginButtonClicked,disabled:!e.nickNameInputText},"Connect"))},t.prototype.onLoginButtonClicked=function(){this.props.state.login()},t.prototype.onNickNameKeyPress=function(e){"Enter"===e.key&&(e.preventDefault(),this.onLoginButtonClicked())},t.prototype.onNickNameChanged=function(e){this.props.state.nickNameInputText=e.currentTarget.value},c.b([l.d.bound],t.prototype,"onLoginButtonClicked",null),c.b([l.d.bound],t.prototype,"onNickNameKeyPress",null),c.b([l.d.bound],t.prototype,"onNickNameChanged",null),t=c.b([p.a],t)}(o.Component)),d=(n(39),n(11)),m=n.n(d);!function(e){e[e.Pending=0]="Pending",e[e.Accepted=1]="Accepted",e[e.Declined=2]="Declined"}(s||(s={}));var h=function(){function e(){this.appointments={}}return e.prototype.appointmentStateChanged=function(e){this.appointments[e.id]=c.a({},this.appointments[e.id],e)},e.prototype.respondToAppointment=function(e,t){var n=t?s.Accepted:s.Declined;m.a.post("/api/appointments/"+e,n,{headers:{"Content-Type":"text/plain"}}).catch(function(e){return alert("Failed to send a response! "+e)})},c.b([l.l],e.prototype,"appointments",void 0),c.b([l.d.bound],e.prototype,"appointmentStateChanged",null),c.b([l.d.bound],e.prototype,"respondToAppointment",null),e}(),g=function(e){function t(){return null!==e&&e.apply(this,arguments)||this}return c.c(t,e),t.prototype.render=function(){var e=this,t=this.props.state;return Object.keys(t.appointments).reverse().map(function(n){var a=t.appointments[n],i="border-warning",r=o.createElement("div",{className:"alert alert-warning status-badge h5"},"Pending..."),c=!1;switch(a.status){case s.Accepted:i="border-success",r=o.createElement("div",{className:"alert alert-success status-badge h5"},"Agreed :)"),c=!0;break;case s.Declined:i="border-danger",r=o.createElement("div",{className:"alert alert-danger status-badge h5"},"Declined :("),c=!0}return o.createElement("div",{className:"card appointment-card "+i,key:n},o.createElement("div",{className:"card-body container-fluid"},o.createElement("div",{className:"row"},o.createElement("div",{className:"col-sm-3"},r),o.createElement("div",{className:"col-sm-5"},"Are you going out with ",e.renderParticipants(a.participants)," ?"),o.createElement("div",{className:"col-sm-4 text-center"},o.createElement("button",{className:"btn btn-success response-button",onClick:function(){return t.respondToAppointment(a.id,!0)},disabled:c},"Yes"),o.createElement("button",{className:"btn btn-danger response-button",onClick:function(){return t.respondToAppointment(a.id,!1)},disabled:c},"No")))))})},t.prototype.renderParticipants=function(e){var t=[],n=e.indexOf(this.props.nickName);n>-1&&(e.splice(n,1),e.length||e.push("yourself"));for(var a=0;a<e.length;a++){var i=a?a===e.length-1?" and ":", ":"";t.push(i),t.push(o.createElement("h1",{className:"badge badge-light participant-badge",key:a},e[a]))}return t},t=c.b([p.a],t)}(o.Component),f=(n(57),function(e){function t(){return null!==e&&e.apply(this,arguments)||this}return c.c(t,e),t.prototype.render=function(){var e=this.props.state;return o.createElement("div",{className:"card appointment-card "},o.createElement("div",{className:"card-body container-fluid"},o.createElement("div",{className:"row"},o.createElement("div",{className:"col-sm-4 description-div"},"Invite some folks to go out tonight:"),o.createElement("div",{className:"col-sm-4"},o.createElement("input",{className:"form-control",type:"search",placeholder:"Comma-separated nicknames...",value:e.nickNamesInputText,onChange:this.onNickNamesChanged,onKeyPress:this.onNickNamesKeyPress})),o.createElement("div",{className:"col-sm-4 text-center"},o.createElement("button",{className:"btn btn-success invite-button",onClick:e.sendInvite,disabled:!e.nickNamesInputText},"Invite")))))},t.prototype.onNickNamesKeyPress=function(e){"Enter"===e.key&&(e.preventDefault(),this.props.state.sendInvite())},t.prototype.onNickNamesChanged=function(e){this.props.state.nickNamesInputText=e.currentTarget.value},c.b([l.d.bound],t.prototype,"onNickNamesKeyPress",null),c.b([l.d.bound],t.prototype,"onNickNamesChanged",null),t=c.b([p.a],t)}(o.Component)),v=function(e){function t(){return null!==e&&e.apply(this,arguments)||this}return c.c(t,e),t.prototype.render=function(){var e=this.props.state;return e.loginState.isLoggedIn?o.createElement("div",null,o.createElement(f,{state:e.sendInviteState}),o.createElement(g,{nickName:e.loginState.nickName,state:e.appointmentsState})):o.createElement("div",null)},t=c.b([p.a],t)}(o.Component),N=n(27),b=function(){function e(e){this.signalRMessageHandler=e,this.nickName="",this.nickNameInputText="",m.a.interceptors.response.use(function(e){return e},function(e){if("Network Error"!==e.message)return Promise.reject(e);location.reload(!0)})}return Object.defineProperty(e.prototype,"isLoggedIn",{get:function(){return!!this.nickName},enumerable:!0,configurable:!0}),e.prototype.login=function(){var e=this,t=(new N.a).withUrl("/api?nick-name="+this.nickNameInputText).build();m.a.defaults.headers.common["x-nick-name"]=this.nickNameInputText,t.on("appointment-state-changed",this.signalRMessageHandler),t.onclose(function(){var e=function(){console.log("Reconnecting to SignalR..."),t.start().then(function(){console.log("Reconnected to SignalR!")},function(){setTimeout(e,5e3)})};e()}),t.start().then(function(){e.nickName=e.nickNameInputText},function(e){alert("Failed to connect to SignalR:  "+JSON.stringify(e))})},c.b([l.l],e.prototype,"nickName",void 0),c.b([l.l],e.prototype,"nickNameInputText",void 0),c.b([l.e],e.prototype,"isLoggedIn",null),c.b([l.d.bound],e.prototype,"login",null),e}(),k=function(){function e(){this.nickNamesInputText=""}return e.prototype.sendInvite=function(){var e=this.nickNamesInputText;this.nickNamesInputText="",m.a.post("/api/new-appointment",e,{headers:{"Content-Type":"text/plain"}}).catch(function(e){alert("Failed to send an invite! "+e)})},c.b([l.l],e.prototype,"nickNamesInputText",void 0),c.b([l.d.bound],e.prototype,"sendInvite",null),e}(),y=new(function(){return function(){this.appointmentsState=new h,this.loginState=new b(this.appointmentsState.appointmentStateChanged),this.sendInviteState=new k}}());a.render(o.createElement("div",null,o.createElement("nav",{className:"navbar navbar-dark bg-dark"},o.createElement("a",{className:"navbar-brand",href:"#"},"Let's Go Out Demo"),o.createElement(u,{state:y.loginState})),o.createElement(v,{state:y})),document.getElementById("root")),function(){if("serviceWorker"in navigator){if(new URL("/api/ui",window.location.toString()).origin!==window.location.origin)return;window.addEventListener("load",function(){var e="/api/ui/service-worker.js";i?(function(e){fetch(e).then(function(t){404===t.status||-1===t.headers.get("content-type").indexOf("javascript")?navigator.serviceWorker.ready.then(function(e){e.unregister().then(function(){window.location.reload()})}):r(e)}).catch(function(){console.log("No internet connection found. App is running in offline mode.")})}(e),navigator.serviceWorker.ready.then(function(){console.log("This web app is being served cache-first by a service worker. To learn more, visit https://goo.gl/SC7cgQ")})):r(e)})}}()}},[[28,2,1]]]);
//# sourceMappingURL=main.8353c0eb.chunk.js.map