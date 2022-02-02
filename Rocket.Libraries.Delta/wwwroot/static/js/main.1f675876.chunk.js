(this["webpackJsonprocket-gundi"]=this["webpackJsonprocket-gundi"]||[]).push([[0],{62:function(t,e,n){},70:function(t,e,n){"use strict";n.r(e);var r,a=n(0),c=n.n(a),o=n(21),i=n.n(o),s=(n(62),n(7)),u=n(4),p=n(1);function h(t){var e,n=Object(a.useState)({path:"/"}),c=Object(s.a)(n,2),o=c[0],i=c[1],u=Object(a.useState)(),h=Object(s.a)(u,2),l=h[0],f=h[1],d=function(){i({path:window.location.pathname})};Object(a.useEffect)((function(){return window.basicRouter.push=function(t){t.path=t.path.toLocaleLowerCase(),i(t)},r=d,window.addEventListener("popstate",r),function(){window.basicRouter.push=function(t){console.error("BasicRouter push was disposed")},window.removeEventListener("popstate",r)}})),Object(a.useEffect)((function(){window.history.pushState(o.data,"",o.path),f(o)}),[o.path,o.data,o]);var b=null!==(e=t.routes.find((function(t){return t.path.toLocaleLowerCase()===o.path})))&&void 0!==e?e:{path:"/404",component:t.badRouteComponent};return l===o?Object(p.jsx)("div",{children:b.component},null===l||void 0===l?void 0:l.path):Object(p.jsx)(p.Fragment,{children:"..."})}window.basicRouter=window.basicRouter||new function t(){Object(u.a)(this,t),this.push=function(t){return console.error("Push not yet initialized")}};var l=function(t){t&&t instanceof Function&&n.e(3).then(n.bind(null,79)).then((function(e){var n=e.getCLS,r=e.getFID,a=e.getFCP,c=e.getLCP,o=e.getTTFB;n(t),r(t),a(t),c(t),o(t)}))},f=(n(64),n(3)),d=n.n(f),b=n(8),j=n(5),v=n(11),y=n(10),g=n(15),x=function(){function t(){Object(u.a)(this,t)}return Object(j.a)(t,[{key:"host",get:function(){return["localhost","127.0.0.1"].includes(window.location.hostname.toLocaleLowerCase())?"http://localhost:5002/":""}}]),t}(),O=function(t){var e=new x;return encodeURI("".concat(e.host,"api/").concat(t))},w=function(){function t(){Object(u.a)(this,t)}return Object(j.a)(t,[{key:"getAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.callAsync(e,"GET");case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"postAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e,n){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.callAsync(e,"POST",n);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e,n){return t.apply(this,arguments)}}()},{key:"callAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e,n,r){var a,c,o,i;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return a=O(e),c=new AbortController,o={method:n,signal:c.signal,headers:{Accept:"application/json","Accept-Encoding":"gzip","Content-Type":"application/json;charset=UTF-8"},body:r?JSON.stringify(r):void 0},setTimeout((function(){return c.abort()}),3e5),t.next=6,fetch(a,o);case 6:if(!(i=t.sent).ok){t.next=11;break}return t.abrupt("return",i);case 11:throw new Error("Error calling api '".concat(a,"'\n HTTP Status Code: '").concat(i.status,"' Status Text '").concat(i.statusText,"'"));case 12:case"end":return t.stop()}}),t)})));return function(e,n,r){return t.apply(this,arguments)}}()}]),t}(),m=function(){function t(){Object(u.a)(this,t)}return Object(j.a)(t,[{key:"getAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e){var n,r;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return n=new w,t.next=3,n.getAsync(e);case 3:return r=t.sent,t.next=6,this.extractPayloadAsync(r);case 6:return t.abrupt("return",t.sent);case 7:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"postAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e,n){var r,a;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return r=new w,t.next=3,r.postAsync(e,n);case 3:return a=t.sent,t.next=6,this.extractPayloadAsync(a);case 6:return t.abrupt("return",t.sent);case 7:case"end":return t.stop()}}),t,this)})));return function(e,n){return t.apply(this,arguments)}}()},{key:"extractPayloadAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e){var n,r;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return n=1,t.next=3,e.json();case 3:if((r=t.sent).code!==n){t.next=8;break}return t.abrupt("return",r.payload);case 8:throw new Error("API call error: "+r.message);case 9:case"end":return t.stop()}}),t)})));return function(e){return t.apply(this,arguments)}}()}]),t}(),k=function(){function t(){Object(u.a)(this,t),this.apiVersion="v1"}return Object(j.a)(t,[{key:"wrappedResponseApiCaller",get:function(){return new m}},{key:"getRelativeUrl",value:function(t){return"".concat(this.apiVersion,"/").concat(this.basePath,"/").concat(t)}},{key:"getAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.wrappedResponseApiCaller.getAsync(this.getRelativeUrl(e));case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"postAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e,n){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.wrappedResponseApiCaller.postAsync(this.getRelativeUrl(e),n);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e,n){return t.apply(this,arguments)}}()}]),t}(),A={zero:0,one:1,negativeOne:-1,strings:{firstElementIndex:0},arrays:{emptyArrayLength:0,firstElementIndex:0},paging:{nextIncrementAmount:1,lastDecrementAmount:-1},zIndexes:{actionBar:2,menuDropDown:function(){return A.zIndexes.actionBar+A.one},mobileTopBar:function(){return A.zIndexes.menuDropDown()+A.one}},defaultGuid:"00000000-0000-0000-0000-000000000000"},D=A,P=n(55),T=function(){function t(e,n,r,a){var c=this;Object(u.a)(this,t),this.maximumPageSize=65535,this.pageSize=this.maximumPageSize,this.hasMorePages=!0,this.page=D.one,this.dataset=void 0,this.pageableEndpoint=void 0,this.searchEndpoint="search",this.applyPagingToSearches=!0,this.debouncingSearcher=void 0,this.searchDebounceMilliseconds=500,this.searchText="",this.page=null!==r&&void 0!==r?r:this.page,this.pageSize=null!==a&&void 0!==a?a:this.pageSize,this.dataset=e,this.pageableEndpoint=null!==n&&void 0!==n?n:"get-pageable",this.debouncingSearcher=Object(P.a)(this.searchDebounceMilliseconds,!1,function(){var t=Object(b.a)(d.a.mark((function t(e,n,r,a){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:if(!e()){t.next=7;break}return c.hasMorePages=!1,t.next=4,n();case 4:a(c.dataset),t.next=12;break;case 7:c.hasMorePages=!0,a([]),c.replaceData([]),c.page=D.one,r();case 12:case"end":return t.stop()}}),t)})));return function(e,n,r,a){return t.apply(this,arguments)}}())}return Object(j.a)(t,[{key:"replaceData",value:function(t){this.dataset.splice(D.zero,this.dataset.length);for(var e=0;e<t.length;e++)this.dataset.push(t[e])}},{key:"search",value:function(t,e,n,r){var a=this;this.searchText=t,this.debouncingSearcher((function(){return a.searchText}),e,n,r)}},{key:"appendData",value:function(t){if(t&&Array.isArray(t)){this.hasMorePages=t.length===this.pageSize,this.page+=1;for(var e=0;e<t.length;e++)this.dataset.push(t[e])}}}]),t}(),R=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(j.a)(n,[{key:"getPaged",value:function(){var t=Object(b.a)(d.a.mark((function t(e){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.getOptionallyPaged(e);case 2:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"searchAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e){var n,r,a,c,o;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return a=null!==(n=e.searchEndpoint)&&void 0!==n?n:"search",e=null!==(r=e)&&void 0!==r?r:new T([]),c="".concat(a,"?searchText=").concat(e.searchText),e.applyPagingToSearches&&(c+="&page=".concat(e.page,"&pageSize=").concat(e.pageSize)),t.next=6,this.getAsync(c);case 6:return o=t.sent,e.replaceData(o),t.abrupt("return",o);case 9:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"getOptionallyPaged",value:function(){var t=Object(b.a)(d.a.mark((function t(e){var n,r,a,c,o;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:if(!(!e||e.hasMorePages)){t.next=11;break}return c=null!==(n=null===(r=e)||void 0===r?void 0:r.pageableEndpoint)&&void 0!==n?n:"get-pageable",e=null!==(a=e)&&void 0!==a?a:new T([]),t.next=6,this.getAsync("".concat(c,"?page=").concat(e.page,"&pageSize=").concat(e.pageSize));case 6:return o=t.sent,e.appendData(o),t.abrupt("return",o);case 11:return t.abrupt("return",[]);case 12:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"insertAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.postAsync("insert",e);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()}]),n}(k),S=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),c=0;c<r;c++)a[c]=arguments[c];return(t=e.call.apply(e,[this].concat(a))).basePath="ProjectDefinitions",t}return n}(R),C=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),c=0;c<r;c++)a[c]=arguments[c];return(t=e.call.apply(e,[this].concat(a))).basePath="Run",t}return Object(j.a)(n,[{key:"runByIdAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e){var n;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.getAsync("run-by-id?projectId=".concat(e));case 2:return n=t.sent,t.abrupt("return",n);case 4:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()}]),n}(R),E=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),c=0;c<r;c++)a[c]=arguments[c];return(t=e.call.apply(e,[this].concat(a))).dataAdapter=new T([],"get-all"),t}return Object(j.a)(n,[{key:"branchlessLabel",get:function(){return"Local Only Projects!"}},{key:"fetchProjectDefinitionsAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(){var e,n=this;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.runAsync({fn:function(){var t=Object(b.a)(d.a.mark((function t(){var e;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,(new S).getOptionallyPaged(n.dataAdapter);case 3:return e=t.sent,n.context.repository.hasFetchedProjectDefinitions=!0,t.abrupt("return",e);case 8:return t.prev=8,t.t0=t.catch(0),console.error(t.t0),setTimeout(Object(b.a)(d.a.mark((function t(){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,n.fetchProjectDefinitionsAsync();case 2:case"end":return t.stop()}}),t)}))),2e3),t.abrupt("return",[]);case 13:case"end":return t.stop()}}),t,null,[[0,8]])})));return function(){return t.apply(this,arguments)}}()});case 2:e=t.sent,this.potter.pushToRepository({projectDefinitions:e});case 4:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()},{key:"tabs",get:function(){var t=[],e=!1;if(this.context.repository.hasFetchedProjectDefinitions)for(var n=0;n<this.context.repository.projectDefinitions.length;n++)if(this.context.repository.projectDefinitions[n].repositoryDetail&&this.context.repository.projectDefinitions[n].repositoryDetail.branch){var r=this.toTitleCase(this.context.repository.projectDefinitions[n].repositoryDetail.branch);-1===t.indexOf(r)&&t.push(r)}else e=!0;return t.sort((function(t,e){return t.toLowerCase()<e.toLowerCase()?-1:1})),e&&t.push(this.branchlessLabel),t}},{key:"getProjectsByBranch",value:function(t){var e,n=this;return t.branch===this.branchlessLabel?this.context.repository.projectDefinitions.filter((function(t){return!t.repositoryDetail||!t.repositoryDetail.branch})):null!==(e=this.context.repository.projectDefinitions.filter((function(e){return!(!e.repositoryDetail||!e.repositoryDetail.branch)&&n.toTitleCase(e.repositoryDetail.branch)===t.branch})))&&void 0!==e?e:[]}},{key:"toTitleCase",value:function(t){if(t){var e=t.charAt(0).toUpperCase(),n=t.substring(1).toLocaleLowerCase();return"".concat(e).concat(n)}return""}},{key:"runProjectAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(e){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.runAsync({fn:function(){var t=Object(b.a)(d.a.mark((function t(){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,(new C).runByIdAsync(e);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t)})));return function(){return t.apply(this,arguments)}}()});case 2:t.sent?this.potter.pushToRepository({message:"Completed successfully"}):this.potter.pushToRepository({message:"Unable to complete running of project"});case 4:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()}]),n}(g.c),F=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),c=0;c<r;c++)a[c]=arguments[c];return(t=e.call.apply(e,[this].concat(a))).projectDefinitions=[],t.message="",t.hasFetchedProjectDefinitions=!1,t.startingUpText="Gundi is starting up. Please wait...",t}return n}(g.d),L=n(77),B=n(78),z=n(73),I=n(74),M={container:{margin:"10px"}},U=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(j.a)(n,[{key:"onRender",value:function(){var t=this;return Object(p.jsx)("div",{style:M.container,children:Object(p.jsx)(L.a,{id:"projects",className:"mb-3",children:this.logic.tabs.map((function(e){var n,r=t.logic.getProjectsByBranch({branch:e}),a=null!==(n=null===r||void 0===r?void 0:r.length)&&void 0!==n?n:0;return Object(p.jsx)(B.a,{eventKey:e,title:"".concat(e," (").concat(a,")"),children:t.table({branch:e})},e)}))})})}},{key:"table",value:function(t){return Object(p.jsx)(z.a,{striped:!0,bordered:!0,hover:!0,size:"sm",responsive:!0,children:Object(p.jsxs)("thead",{children:[Object(p.jsxs)("tr",{children:[Object(p.jsx)("th",{children:"#"}),Object(p.jsx)("th",{children:"Project"}),Object(p.jsx)("th",{children:"Action"})]}),this.logic.getProjectsByBranch(t).map((function(t,e){return Object(p.jsxs)("tr",{children:[Object(p.jsx)("td",{children:e+1}),Object(p.jsx)("td",{children:t.label}),Object(p.jsx)("td",{children:Object(p.jsx)(I.a,{onClick:function(){window.basicRouter.push({path:"/project",data:t})},children:"Run"})})]},e)}))]})})}}]),n}(g.a),G=n(76),J=n(75);function N(t){return Object(p.jsx)(G.a,{show:t.show,children:Object(p.jsx)(J.a,{animated:!0,now:100})})}var V=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;return Object(u.a)(this,n),(t=e.call(this,new F,{},new E)).componentToShow=function(){return t.repository.hasFetchedProjectDefinitions?Object(p.jsx)(U,{potter:t.potter},t.getChildKeyFromObject(t.repository.projectDefinitions)):Object(p.jsx)("div",{style:{width:"100%",margin:"auto",border:"solid 1px #DFDFDF",textAlign:"center",paddingTop:"50px"},children:t.repository.startingUpText})},t}return Object(j.a)(n,[{key:"message",value:function(){var t=this;return Object(p.jsxs)(G.a,{show:!!this.repository.message,onHide:function(){return t.potter.pushToRepository({message:""})},children:[Object(p.jsx)(G.a.Body,{children:this.repository.message}),Object(p.jsx)(G.a.Footer,{children:Object(p.jsx)(I.a,{variant:"secondary",onClick:function(){return t.potter.pushToRepository({message:""})},children:"Close"})})]})}},{key:"componentDidMount",value:function(){var t=Object(b.a)(d.a.mark((function t(){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.logic.fetchProjectDefinitionsAsync();case 2:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()},{key:"onRender",value:function(){return Object(p.jsxs)(p.Fragment,{children:[Object(p.jsx)(N,{show:this.repository.busy}),this.componentToShow()]})}},{key:"onStartedAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:case"end":return t.stop()}}),t)})));return function(){return t.apply(this,arguments)}}()}]),n}(g.b);var H=function t(e){var n=this;Object(u.a)(this,t),this.eventSource=void 0,this.eventSource=new EventSource(e.url),this.eventSource.onmessage=function(t){"---terminate---"===t.data&&n.eventSource?(n.eventSource.close(),n.eventSource.onmessage=null,e.onCompleted()):e.onData(t.data)}},K=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),c=0;c<r;c++)a[c]=arguments[c];return(t=e.call.apply(e,[this].concat(a))).isRunning=!1,t}return Object(j.a)(n,[{key:"projectDefinition",get:function(){return window.history.state}},{key:"runProjectAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(){var e,n;return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:if(t.prev=0,!this.isRunning){t.next=3;break}return t.abrupt("return");case 3:return this.potter.pushToRepository({busy:!0}),this.handleEventListening(),this.isRunning=!0,t.next=8,(new C).runByIdAsync(this.projectDefinition.projectId);case 8:e=t.sent,this.potter.pushToRepository({processRunningResult:e}),t.next=17;break;case 12:t.prev=12,t.t0=t.catch(0),console.error(t.t0),n={errors:["Error occured on server"]},this.potter.pushToRepository({processRunningResult:[n]});case 17:return t.prev=17,this.isRunning=!1,t.finish(17);case 20:case"end":return t.stop()}}),t,this,[[0,12,17,20]])})));return function(){return t.apply(this,arguments)}}()},{key:"scrollToBottom",value:function(){var t=document.getElementById("gundi-output");t&&t.scrollIntoView({behavior:"smooth",block:"end",inline:"nearest"})}},{key:"handleEventListening",value:function(){var t=this;new H({url:"".concat((new x).host,"api/v1/EventQueue/listen?projectId=").concat(this.projectDefinition.projectId),onData:function(e){t.context.repository.output.push(e),t.potter.pushToRepository({}),t.scrollToBottom()},onCompleted:function(){t.potter.pushToRepository({busy:!1}),t.scrollToBottom()}})}}]),n}(g.c),Q=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),c=0;c<r;c++)a[c]=arguments[c];return(t=e.call.apply(e,[this].concat(a))).processRunningResult=[],t.output=[],t}return n}(g.d);function W(t){return document.title=t.title,Object(p.jsxs)(p.Fragment,{children:[Object(p.jsx)(N,{show:t.busy}),t.children]})}var q={header:{fontSize:"25px",fontWeight:"bold"}},X=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.call(this,new Q,{},new K)}return Object(j.a)(n,[{key:"componentDidMount",value:function(){var t=Object(b.a)(d.a.mark((function t(){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.logic.runProjectAsync();case 2:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()},{key:"onRender",value:function(){return Object(p.jsxs)(W,{title:"Gundi - ".concat(this.logic.projectDefinition.label),busy:this.repository.busy,children:[Object(p.jsx)("div",{style:q.header,children:this.logic.projectDefinition.label}),Object(p.jsx)("hr",{}),Object(p.jsx)("div",{id:"gundi-output",style:{padding:"10px",margin:"4px",border:"solid 1px #DFDFDF"},children:Object(p.jsx)("ol",{children:this.repository.output.map((function(t){return t.split("<br/>").map((function(t,e){return t?Object(p.jsx)("li",{children:t},e):Object(p.jsx)(p.Fragment,{})}))}))})})]})}},{key:"onStartedAsync",value:function(){var t=Object(b.a)(d.a.mark((function t(){return d.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:case"end":return t.stop()}}),t)})));return function(){return t.apply(this,arguments)}}()}]),n}(g.b);i.a.render(Object(p.jsx)(c.a.StrictMode,{children:Object(p.jsx)(h,{routes:[{path:"/",component:Object(p.jsx)(V,{})},{path:"/project",component:Object(p.jsx)(X,{})}],badRouteComponent:Object(p.jsx)("div",{children:"Nothing here"})})}),document.getElementById("root")),l()}},[[70,1,2]]]);
//# sourceMappingURL=main.1f675876.chunk.js.map