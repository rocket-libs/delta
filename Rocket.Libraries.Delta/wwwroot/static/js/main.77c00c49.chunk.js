(this["webpackJsonprocket-gundi"]=this["webpackJsonprocket-gundi"]||[]).push([[0],{108:function(t,e,n){"use strict";n.r(e);var r,a=n(0),i=n.n(a),c=n(17),o=n.n(c),s=(n(70),n(12)),u=n(3),p=n(1);function l(t){var e,n=Object(a.useState)({path:"/"}),i=Object(s.a)(n,2),c=i[0],o=i[1],u=Object(a.useState)(),l=Object(s.a)(u,2),h=l[0],d=l[1],f=function(){o({path:window.location.pathname})};Object(a.useEffect)((function(){return window.basicRouter.push=function(t){t.path=t.path.toLocaleLowerCase(),o(t)},r=f,window.addEventListener("popstate",r),function(){window.basicRouter.push=function(t){console.error("BasicRouter push was disposed")},window.removeEventListener("popstate",r)}})),Object(a.useEffect)((function(){window.history.pushState(c.data,"",c.path),d(c)}),[c.path,c.data,c]);var j=null!==(e=t.routes.find((function(t){return t.path.toLocaleLowerCase()===c.path})))&&void 0!==e?e:{path:"/404",component:t.badRouteComponent};return h===c?Object(p.jsx)("div",{children:j.component},null===h||void 0===h?void 0:h.path):Object(p.jsx)(p.Fragment,{children:"..."})}window.basicRouter=window.basicRouter||new function t(){Object(u.a)(this,t),this.push=function(t){return console.error("Push not yet initialized")}};var h=function(t){t&&t instanceof Function&&n.e(3).then(n.bind(null,119)).then((function(e){var n=e.getCLS,r=e.getFID,a=e.getFCP,i=e.getLCP,c=e.getTTFB;n(t),r(t),a(t),i(t),c(t)}))},d=(n(72),n(4)),f=n.n(d),j=n(10),b=n(5),v=n(9),y=n(8),O=n(16),g=function(){function t(){Object(u.a)(this,t)}return Object(b.a)(t,[{key:"host",get:function(){return["localhost","127.0.0.1"].includes(window.location.hostname.toLocaleLowerCase())?"http://localhost:5002/":""}}]),t}(),x=function(t){var e=new g;return encodeURI("".concat(e.host,"api/").concat(t))},w=function(){function t(){Object(u.a)(this,t)}return Object(b.a)(t,[{key:"getAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.callAsync(e,"GET");case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"postAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e,n){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.callAsync(e,"POST",n);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e,n){return t.apply(this,arguments)}}()},{key:"callAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e,n,r){var a,i,c,o;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return a=x(e),i=new AbortController,c={method:n,signal:i.signal,headers:{Accept:"application/json","Accept-Encoding":"gzip","Content-Type":"application/json;charset=UTF-8"},body:r?JSON.stringify(r):void 0},setTimeout((function(){return i.abort()}),3e5),t.next=6,fetch(a,c);case 6:if(!(o=t.sent).ok){t.next=11;break}return t.abrupt("return",o);case 11:throw new Error("Error calling api '".concat(a,"'\n HTTP Status Code: '").concat(o.status,"' Status Text '").concat(o.statusText,"'"));case 12:case"end":return t.stop()}}),t)})));return function(e,n,r){return t.apply(this,arguments)}}()}]),t}(),m=function(){function t(){Object(u.a)(this,t)}return Object(b.a)(t,[{key:"getAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){var n,r;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return n=new w,t.next=3,n.getAsync(e);case 3:return r=t.sent,t.next=6,this.extractPayloadAsync(r);case 6:return t.abrupt("return",t.sent);case 7:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"postAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e,n){var r,a;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return r=new w,t.next=3,r.postAsync(e,n);case 3:return a=t.sent,t.next=6,this.extractPayloadAsync(a);case 6:return t.abrupt("return",t.sent);case 7:case"end":return t.stop()}}),t,this)})));return function(e,n){return t.apply(this,arguments)}}()},{key:"extractPayloadAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){var n,r;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return n=1,t.next=3,e.json();case 3:if((r=t.sent).code!==n){t.next=8;break}return t.abrupt("return",r.payload);case 8:throw new Error("API call error: "+r.message);case 9:case"end":return t.stop()}}),t)})));return function(e){return t.apply(this,arguments)}}()}]),t}(),k=function(){function t(){Object(u.a)(this,t),this.apiVersion="v1"}return Object(b.a)(t,[{key:"wrappedResponseApiCaller",get:function(){return new m}},{key:"getRelativeUrl",value:function(t){return"".concat(this.apiVersion,"/").concat(this.basePath,"/").concat(t)}},{key:"getAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.wrappedResponseApiCaller.getAsync(this.getRelativeUrl(e));case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"postAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e,n){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.wrappedResponseApiCaller.postAsync(this.getRelativeUrl(e),n);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e,n){return t.apply(this,arguments)}}()}]),t}(),A={zero:0,one:1,negativeOne:-1,strings:{firstElementIndex:0},arrays:{emptyArrayLength:0,firstElementIndex:0},paging:{nextIncrementAmount:1,lastDecrementAmount:-1},zIndexes:{actionBar:2,menuDropDown:function(){return A.zIndexes.actionBar+A.one},mobileTopBar:function(){return A.zIndexes.menuDropDown()+A.one}},defaultGuid:"00000000-0000-0000-0000-000000000000"},D=A,S=n(61),P=function(){function t(e,n,r,a){var i=this;Object(u.a)(this,t),this.maximumPageSize=65535,this.pageSize=this.maximumPageSize,this.hasMorePages=!0,this.page=D.one,this.dataset=void 0,this.pageableEndpoint=void 0,this.searchEndpoint="search",this.applyPagingToSearches=!0,this.debouncingSearcher=void 0,this.searchDebounceMilliseconds=500,this.searchText="",this.page=null!==r&&void 0!==r?r:this.page,this.pageSize=null!==a&&void 0!==a?a:this.pageSize,this.dataset=e,this.pageableEndpoint=null!==n&&void 0!==n?n:"get-pageable",this.debouncingSearcher=Object(S.a)(this.searchDebounceMilliseconds,!1,function(){var t=Object(j.a)(f.a.mark((function t(e,n,r,a){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:if(!e()){t.next=7;break}return i.hasMorePages=!1,t.next=4,n();case 4:a(i.dataset),t.next=12;break;case 7:i.hasMorePages=!0,a([]),i.replaceData([]),i.page=D.one,r();case 12:case"end":return t.stop()}}),t)})));return function(e,n,r,a){return t.apply(this,arguments)}}())}return Object(b.a)(t,[{key:"replaceData",value:function(t){this.dataset.splice(D.zero,this.dataset.length);for(var e=0;e<t.length;e++)this.dataset.push(t[e])}},{key:"search",value:function(t,e,n,r){var a=this;this.searchText=t,this.debouncingSearcher((function(){return a.searchText}),e,n,r)}},{key:"appendData",value:function(t){if(t&&Array.isArray(t)){this.hasMorePages=t.length===this.pageSize,this.page+=1;for(var e=0;e<t.length;e++)this.dataset.push(t[e])}}}]),t}(),C=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"getPaged",value:function(){var t=Object(j.a)(f.a.mark((function t(e){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.getOptionallyPaged(e);case 2:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"searchAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){var n,r,a,i,c;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return a=null!==(n=e.searchEndpoint)&&void 0!==n?n:"search",e=null!==(r=e)&&void 0!==r?r:new P([]),i="".concat(a,"?searchText=").concat(e.searchText),e.applyPagingToSearches&&(i+="&page=".concat(e.page,"&pageSize=").concat(e.pageSize)),t.next=6,this.getAsync(i);case 6:return c=t.sent,e.replaceData(c),t.abrupt("return",c);case 9:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"getOptionallyPaged",value:function(){var t=Object(j.a)(f.a.mark((function t(e){var n,r,a,i,c;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:if(!(!e||e.hasMorePages)){t.next=11;break}return i=null!==(n=null===(r=e)||void 0===r?void 0:r.pageableEndpoint)&&void 0!==n?n:"get-pageable",e=null!==(a=e)&&void 0!==a?a:new P([]),t.next=6,this.getAsync("".concat(i,"?page=").concat(e.page,"&pageSize=").concat(e.pageSize));case 6:return c=t.sent,e.appendData(c),t.abrupt("return",c);case 11:return t.abrupt("return",[]);case 12:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"insertAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.postAsync("insert",e);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()},{key:"updateAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.postAsync("update",e);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()}]),n}(k),R=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).basePath="ProjectDefinitions",t}return n}(C),T=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).basePath="Run",t}return Object(b.a)(n,[{key:"runByIdAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){var n;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.getAsync("run-by-id?projectId=".concat(e));case 2:return n=t.sent,t.abrupt("return",n);case 4:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()}]),n}(C),L=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).dataAdapter=new P([],"get-all"),t}return Object(b.a)(n,[{key:"branchlessLabel",get:function(){return"Local Only Projects!"}},{key:"fetchProjectDefinitionsAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(){var e,n=this;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.runAsync({fn:function(){var t=Object(j.a)(f.a.mark((function t(){var e;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.prev=0,t.next=3,(new R).getOptionallyPaged(n.dataAdapter);case 3:return e=t.sent,n.context.repository.hasFetchedProjectDefinitions=!0,t.abrupt("return",e);case 8:return t.prev=8,t.t0=t.catch(0),console.error(t.t0),setTimeout(Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,n.fetchProjectDefinitionsAsync();case 2:case"end":return t.stop()}}),t)}))),2e3),t.abrupt("return",[]);case 13:case"end":return t.stop()}}),t,null,[[0,8]])})));return function(){return t.apply(this,arguments)}}()});case 2:e=t.sent,this.potter.pushToRepository({projectDefinitions:e});case 4:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()},{key:"tabs",get:function(){var t=[],e=!1;if(this.context.repository.hasFetchedProjectDefinitions)for(var n=0;n<this.context.repository.projectDefinitions.length;n++)if(this.context.repository.projectDefinitions[n].repositoryDetail&&this.context.repository.projectDefinitions[n].repositoryDetail.branch){var r=this.toTitleCase(this.context.repository.projectDefinitions[n].repositoryDetail.branch);-1===t.indexOf(r)&&t.push(r)}else e=!0;return t.sort((function(t,e){return t.toLowerCase()<e.toLowerCase()?-1:1})),e&&t.push(this.branchlessLabel),t}},{key:"getProjectsByBranch",value:function(t){var e,n=this;return t.branch===this.branchlessLabel?this.context.repository.projectDefinitions.filter((function(t){return!t.repositoryDetail||!t.repositoryDetail.branch})):null!==(e=this.context.repository.projectDefinitions.filter((function(e){return!(!e.repositoryDetail||!e.repositoryDetail.branch)&&n.toTitleCase(e.repositoryDetail.branch)===t.branch})))&&void 0!==e?e:[]}},{key:"toTitleCase",value:function(t){if(t){var e=t.charAt(0).toUpperCase(),n=t.substring(1).toLocaleLowerCase();return"".concat(e).concat(n)}return""}},{key:"runProjectAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.runAsync({fn:function(){var t=Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,(new T).runByIdAsync(e);case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t)})));return function(){return t.apply(this,arguments)}}()});case 2:t.sent?this.potter.pushToRepository({message:"Completed successfully"}):this.potter.pushToRepository({message:"Unable to complete running of project"});case 4:case"end":return t.stop()}}),t,this)})));return function(e){return t.apply(this,arguments)}}()}]),n}(O.c),M=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).projectDefinitions=[],t.message="",t.hasFetchedProjectDefinitions=!1,t.startingUpText="Gundi is starting up. Please wait...",t}return n}(O.d),F=n(116),E=n(118),B=n(111),N=n(112),I=n(113),z={container:{margin:"10px"}},V=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"onRender",value:function(){var t=this;return Object(p.jsx)("div",{style:z.container,children:Object(p.jsx)(F.a,{id:"projects",className:"mb-3",children:this.logic.tabs.map((function(e){var n,r=t.logic.getProjectsByBranch({branch:e}),a=null!==(n=null===r||void 0===r?void 0:r.length)&&void 0!==n?n:0;return Object(p.jsx)(E.a,{eventKey:e,title:"".concat(e," (").concat(a,")"),children:t.table({branch:e})},e)}))})})}},{key:"table",value:function(t){return Object(p.jsx)(B.a,{striped:!0,bordered:!0,hover:!0,size:"sm",responsive:!0,children:Object(p.jsxs)("thead",{children:[Object(p.jsxs)("tr",{children:[Object(p.jsx)("th",{children:"#"}),Object(p.jsx)("th",{children:"Project"}),Object(p.jsx)("th",{children:"Action"})]}),this.logic.getProjectsByBranch(t).map((function(t,e){return Object(p.jsxs)("tr",{children:[Object(p.jsx)("td",{children:e+1}),Object(p.jsx)("td",{children:t.label}),Object(p.jsx)("td",{children:Object(p.jsxs)(N.a,{children:[Object(p.jsx)(I.a,{onClick:function(){window.basicRouter.push({path:"/project",data:t})},children:"Run"}),Object(p.jsx)(I.a,{variant:"info",onClick:function(){window.basicRouter.push({path:"/project-definitions/configure",data:t})},children:"Configure"})]})})]},e)}))]})})}}]),n}(O.a),U=n(115),G=n(114);function W(t){return Object(p.jsx)(U.a,{show:t.show,children:Object(p.jsx)(G.a,{animated:!0,now:100})})}var J=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"render",value:function(){return window.document.title=this.props.title,Object(p.jsxs)(p.Fragment,{children:[Object(p.jsx)("h1",{children:this.props.title}),Object(p.jsx)("hr",{})]})}}]),n}(a.PureComponent),K=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;return Object(u.a)(this,n),(t=e.call(this,new M,{},new L)).componentToShow=function(){return t.repository.hasFetchedProjectDefinitions?Object(p.jsx)(V,{potter:t.potter},t.getChildKeyFromObject(t.repository.projectDefinitions)):Object(p.jsx)("div",{style:{width:"100%",margin:"auto",border:"solid 1px #DFDFDF",textAlign:"center",paddingTop:"50px"},children:t.repository.startingUpText})},t}return Object(b.a)(n,[{key:"message",value:function(){var t=this;return Object(p.jsxs)(U.a,{show:!!this.repository.message,onHide:function(){return t.potter.pushToRepository({message:""})},children:[Object(p.jsx)(U.a.Body,{children:this.repository.message}),Object(p.jsx)(U.a.Footer,{children:Object(p.jsx)(I.a,{variant:"secondary",onClick:function(){return t.potter.pushToRepository({message:""})},children:"Close"})})]})}},{key:"componentDidMount",value:function(){var t=Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.logic.fetchProjectDefinitionsAsync();case 2:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()},{key:"onRender",value:function(){return Object(p.jsxs)(p.Fragment,{children:[Object(p.jsx)(J,{title:"Projects"}),Object(p.jsx)(W,{show:this.repository.busy}),this.componentToShow()]})}},{key:"onStartedAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:case"end":return t.stop()}}),t)})));return function(){return t.apply(this,arguments)}}()}]),n}(O.b);function H(){return window.history.state}var Q=function t(e){var n=this;Object(u.a)(this,t),this.eventSource=void 0,this.eventSource=new EventSource(e.url),this.eventSource.onmessage=function(t){"---terminate---"===t.data&&n.eventSource?(n.eventSource.close(),n.eventSource.onmessage=null,e.onCompleted()):e.onData(t.data)}},q=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).isRunning=!1,t}return Object(b.a)(n,[{key:"projectDefinition",get:function(){return H()}},{key:"runProjectAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(){var e,n;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:if(t.prev=0,!this.isRunning){t.next=3;break}return t.abrupt("return");case 3:return this.potter.pushToRepository({busy:!0}),this.handleEventListening(),this.isRunning=!0,t.next=8,(new T).runByIdAsync(this.projectDefinition.projectId);case 8:e=t.sent,this.potter.pushToRepository({processRunningResult:e}),t.next=17;break;case 12:t.prev=12,t.t0=t.catch(0),console.error(t.t0),n={errors:["Error occured on server"]},this.potter.pushToRepository({processRunningResult:[n]});case 17:return t.prev=17,this.isRunning=!1,t.finish(17);case 20:case"end":return t.stop()}}),t,this,[[0,12,17,20]])})));return function(){return t.apply(this,arguments)}}()},{key:"hasOutput",get:function(){return this.context.repository.output&&this.context.repository.output.length>0}},{key:"scrollToBottom",value:function(){var t=document.getElementById("gundi-output");t&&t.scrollIntoView({behavior:"smooth",block:"end",inline:"nearest"})}},{key:"handleEventListening",value:function(){var t=this;new Q({url:"".concat((new g).host,"api/v1/EventQueue/listen?projectId=").concat(this.projectDefinition.projectId),onData:function(e){t.context.repository.output.push(e),t.potter.pushToRepository({}),t.scrollToBottom()},onCompleted:function(){t.potter.pushToRepository({busy:!1}),t.scrollToBottom()}})}}]),n}(O.c),X=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).processRunningResult=[],t.output=[],t}return n}(O.d);function Y(t){return document.title=t.title,Object(p.jsxs)(p.Fragment,{children:[Object(p.jsx)(W,{show:t.busy}),t.children]})}var Z=n(63),$=n.n(Z),_={header:{fontSize:"25px",fontWeight:"bold"}},tt=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.call(this,new X,{},new q)}return Object(b.a)(n,[{key:"componentDidMount",value:function(){var t=Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.logic.runProjectAsync();case 2:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()},{key:"onRender",value:function(){return Object(p.jsxs)(Y,{title:"Gundi - ".concat(this.logic.projectDefinition.label),busy:this.repository.busy,children:[Object(p.jsx)("div",{style:_.header,children:this.logic.projectDefinition.label}),Object(p.jsx)("hr",{}),Object(p.jsx)("div",{id:"gundi-output",style:{padding:"10px",margin:"4px",border:"solid 1px #DFDFDF"},children:Object(p.jsx)($.a,{children:Object(p.jsx)("ol",{children:this.repository.output.map((function(t){return t.split("<br/>").map((function(t,e){return t?Object(p.jsx)("li",{children:t},e):Object(p.jsx)(p.Fragment,{})}))}))})})})]})}},{key:"onStartedAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:case"end":return t.stop()}}),t)})));return function(){return t.apply(this,arguments)}}()}]),n}(O.b),et=n(34),nt=n.n(et),rt=function t(){Object(u.a)(this,t),this.applicationName="Gundi",this.save="Save"},at=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).addNewApplication="Add New Application",t}return n}(rt),it=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).repository=void 0,t.model=void 0,t.strings=new at,t}return n}(nt.a),ct=(n(56),{container:{width:"90%",margin:"auto"}}),ot=new it,st=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"render",value:function(){return Object(p.jsxs)("div",{style:ct.container,children:[Object(p.jsx)(J,{title:"Welcome to "+ot.strings.applicationName}),Object(p.jsx)("div",{className:"flex-grid-responsive",children:Object(p.jsx)("div",{className:"col1",children:Object(p.jsx)(I.a,{onClick:function(){return window.basicRouter.push({path:"/applications/form"})},children:ot.strings.addNewApplication})})})]})}}]),n}(a.PureComponent),ut=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"render",value:function(){return Object(p.jsxs)("div",{className:"flex-grid-responsive",children:[Object(p.jsxs)("div",{className:"col1",children:[Object(p.jsx)("div",{children:Object(p.jsx)(I.a,{variant:"link",onClick:function(){return window.basicRouter.push({path:"/"})},children:"Home"})}),Object(p.jsx)("div",{children:Object(p.jsx)(I.a,{variant:"link",onClick:function(){return window.basicRouter.push({path:"/project-definitions/list"})},children:"Projects List"})})]}),Object(p.jsx)("div",{className:"col6",children:this.props.children})]})}}]),n}(a.PureComponent),pt=n(2),lt=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"displayLabel",get:function(){return this.props.displayLabel?Object(p.jsx)("div",{children:this.props.displayLabel}):null}},{key:"render",value:function(){return Object(p.jsxs)(p.Fragment,{children:[this.displayLabel,this.props.children]})}}]),n}(a.PureComponent),ht=n(64),dt=function(){function t(){Object(u.a)(this,t)}return Object(b.a)(t,[{key:"onSelectionChanged",value:function(t){return t}},{key:"adaptManyToDropdownValue",value:function(t){return t}},{key:"extractManyDataFromDropdownValue",value:function(t){return t}},{key:"adaptSingleToDropdownValue",value:function(t){return t}},{key:"extractSingleDataFromDropdownValue",value:function(t){return t}}]),t}(),ft=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).dropDownValues=[],t}return Object(b.a)(n,[{key:"adapter",get:function(){return this.props.adapter?this.props.adapter:new dt}},{key:"render",value:function(){var t=this;return Object(p.jsx)(lt,Object(pt.a)(Object(pt.a)({},this.props),{},{children:Object(p.jsx)(ht.a,Object(pt.a)(Object(pt.a)({closeMenuOnSelect:!0,options:this.props.data.map((function(e){return t.adapter.adaptSingleToDropdownValue(e)}))},this.props.adapter),{},{onChange:function(e){if(t.props.onSelectionChanged){var n=t.adapter.onSelectionChanged(e);t.props.onSelectionChanged(n)}}}))}))}}]),n}(a.PureComponent),jt=function(){function t(e){var n=arguments.length>1&&void 0!==arguments[1]?arguments[1]:{};Object(u.a)(this,t),this.isMulti=void 0,this.closeMenuOnSelect=void 0,this.labelKey=void 0,this.labelKey=e,this.isMulti=n.isMulti,this.closeMenuOnSelect=n.closeMenuOnSelect}return Object(b.a)(t,[{key:"adaptManyToDropdownValue",value:function(t){var e=this;return t.map((function(t){return e.adaptSingleToDropdownValue(t)}))}},{key:"extractManyDataFromDropdownValue",value:function(t){var e=this;return t.map((function(t){return e.extractSingleDataFromDropdownValue(t)}))}},{key:"adaptSingleToDropdownValue",value:function(t){return{value:t,label:this.getLabel(t)}}},{key:"extractSingleDataFromDropdownValue",value:function(t){return null===t||void 0===t?void 0:t.value}},{key:"onSelectionChanged",value:function(t){var e=this,n=(this.isMulti,t);return this.isMulti?n.map((function(t){return e.extractSingleDataFromDropdownValue(t)})):this.extractSingleDataFromDropdownValue(n)}},{key:"getLabel",value:function(t){if(t){var e=Reflect.getOwnPropertyDescriptor(t,this.labelKey);if(e){var n=e.value;if(n)return n.toString()}}return""}}]),t}(),bt={form:{width:"90%",margin:"auto",padding:"10px"},buttonsContainer:{textAlign:"right"}},vt=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"buttons",get:function(){return this.props.buttons?Object(p.jsxs)("div",{style:bt.buttonsContainer,children:[Object(p.jsx)("hr",{}),this.props.buttons]}):null}},{key:"render",value:function(){return Object(p.jsxs)("div",{style:bt.form,children:[Object(p.jsxs)("div",{children:[Object(p.jsx)("h1",{children:this.props.title}),Object(p.jsx)("div",{children:this.props.description}),Object(p.jsx)("hr",{})]}),this.props.children,this.buttons]})}}]),n}(a.PureComponent),yt=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"render",value:function(){var t=this;return Object(p.jsx)(lt,Object(pt.a)(Object(pt.a)({},this.props),{},{children:Object(p.jsx)("input",{type:"text",onChange:function(e){return t.props.onChange(e.target.value)},defaultValue:this.props.value})}))}}]),n}(a.PureComponent),Ot=function t(){Object(u.a)(this,t),this.applicationProjectDefinitions=[],this.validationErrors=[]},gt=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).name="Application Name",t.addNewApplication="Add New Application",t.description="Use this form to add a new application.",t.projects="Projects",t}return n}(rt),xt=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).strings=new gt,t.repository=new Ot,t.model={},t.dataAdapter=new P([],"get-all"),t}return Object(b.a)(n,[{key:"onProjectSelectionChanged",value:function(t){this.updateModel({applicationProjectDefinitions:t})}},{key:"setApplicationDisplayLabel",value:function(t){this.updateModel({displayLabel:t})}},{key:"fetchProjectDefinitionsAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(){var e,n;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,(new R).getOptionallyPaged(this.dataAdapter);case 2:e=t.sent,(n=e.filter((function(t){return!!t.repositoryDetail})).map((function(t){return{label:t.repositoryDetail.branch+" \u2212 "+t.label,repositoryDetail:t.repositoryDetail,tag:""}}))).sort((function(t,e){return t.label.localeCompare(e.label)})),this.updateRepository({applicationProjectDefinitions:n});case 6:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()},{key:"saveApplicationAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(){var e;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return e=this.model,t.abrupt("return",Promise.resolve(e));case 2:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()}]),n}(nt.a),wt=new xt,mt=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"componentDidMount",value:function(){var t=Object(j.a)(f.a.mark((function t(){var e=this;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return wt.setRerender((function(){return e.forceUpdate()})),t.next=3,wt.fetchProjectDefinitionsAsync();case 3:case"end":return t.stop()}}),t)})));return function(){return t.apply(this,arguments)}}()},{key:"saveButton",get:function(){return Object(p.jsx)(I.a,{onClick:Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,wt.saveApplicationAsync();case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t)}))),children:wt.strings.save})}},{key:"render",value:function(){return Object(p.jsxs)(vt,{title:wt.strings.addNewApplication,description:wt.strings.description,buttons:this.saveButton,children:[Object(p.jsx)(yt,{displayLabel:wt.strings.name,validationErrors:wt.repository.validationErrors,id:"Application.DisplayLabel",onChange:function(t){return wt.setApplicationDisplayLabel(t)},value:wt.model.displayLabel}),Object(p.jsx)(ft,{id:"Application.Projects",displayLabel:wt.strings.projects,data:wt.repository.applicationProjectDefinitions,validationErrors:wt.repository.validationErrors,selected:wt.model.applicationProjectDefinitions,onSelectionChanged:function(t){wt.onProjectSelectionChanged(t)},adapter:new jt("label",{isMulti:!0,closeMenuOnSelect:!1})})]})}}]),n}(a.PureComponent),kt=n(117),At=function(){function t(e){Object(u.a)(this,t),this.runStateWriter=void 0,this.runStateWriter=e.runStateWriter}return Object(b.a)(t,[{key:"runAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(e){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.prev=0,this.runStateWriter(!0),t.next=4,e();case 4:return t.abrupt("return",t.sent);case 5:return t.prev=5,this.runStateWriter(!1),t.finish(5);case 8:case"end":return t.stop()}}),t,this,[[0,,5,8]])})));return function(e){return t.apply(this,arguments)}}()}]),t}(),Dt=function t(){Object(u.a)(this,t),this.busy=!1,this.savedModel=""},St=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){var t;Object(u.a)(this,n);for(var r=arguments.length,a=new Array(r),i=0;i<r;i++)a[i]=arguments[i];return(t=e.call.apply(e,[this].concat(a))).repository=new Dt,t.model={},t.asyncRunner=new At({runStateWriter:function(e){return t.updateRepository({busy:e})}}),t}return Object(b.a)(n,[{key:"submitAsync",value:function(){var t=Object(j.a)(f.a.mark((function t(){var e=this;return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,this.asyncRunner.runAsync(Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,(new R).updateAsync(e.model);case 2:e.repository.savedModel=JSON.stringify(e.model);case 3:case"end":return t.stop()}}),t)}))));case 2:case"end":return t.stop()}}),t,this)})));return function(){return t.apply(this,arguments)}}()},{key:"modelChanged",get:function(){return JSON.stringify(this.model)!==this.repository.savedModel}}]),n}(nt.a),Pt=new St,Ct=function(t){Object(v.a)(n,t);var e=Object(y.a)(n);function n(){return Object(u.a)(this,n),e.apply(this,arguments)}return Object(b.a)(n,[{key:"componentDidMount",value:function(){var t=this;Pt.model=H(),Pt.repository.savedModel=JSON.stringify(Pt.model),Pt.setRerender((function(){return t.forceUpdate()})),this.forceUpdate()}},{key:"render",value:function(){return Object(p.jsxs)(p.Fragment,{children:[Object(p.jsx)(W,{show:Pt.repository.busy}),Object(p.jsx)(J,{title:"Configure: "+Pt.model.label}),Object(p.jsx)("div",{style:{width:"90%",marginTop:"30px"},children:Object(p.jsxs)(kt.a,{children:[Pt.model.project?Object(p.jsxs)(kt.a.Group,{className:"mb-3",controlId:"publishUrl",children:[Object(p.jsx)(kt.a.Label,{children:"Publish URL"}),Object(p.jsx)(kt.a.Control,{type:"text",placeholder:"Enter url to publish to",value:Pt.model.project.publishUrl,onChange:function(t){return Pt.updateModel({project:Object(pt.a)(Object(pt.a)({},Pt.model.project),{},{publishUrl:t.target.value})})}}),Object(p.jsx)(kt.a.Text,{className:"text-muted",children:"Git remote to which build artifacts are pushed to on successful builds."})]}):null,Object(p.jsx)(kt.a.Group,{className:"mb-3",controlId:"formBasicCheckbox",children:Object(p.jsx)(kt.a.Check,{type:"switch",label:"Delete Local Repository After Build Success",checked:!Pt.model.keepSource,onChange:function(){return Pt.updateModel({keepSource:!Pt.model.keepSource})}})}),Object(p.jsx)(I.a,{onClick:Object(j.a)(f.a.mark((function t(){return f.a.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.next=2,Pt.submitAsync();case 2:return t.abrupt("return",t.sent);case 3:case"end":return t.stop()}}),t)}))),disabled:!Pt.modelChanged,children:"Save"})]})})]})}}]),n}(a.PureComponent);o.a.render(Object(p.jsx)(i.a.StrictMode,{children:Object(p.jsx)(ut,{children:Object(p.jsx)(l,{routes:[{path:"/",component:Object(p.jsx)(st,{})},{path:"/project-definitions/list",component:Object(p.jsx)(K,{})},{path:"/project",component:Object(p.jsx)(tt,{})},{path:"/applications/form",component:Object(p.jsx)(mt,{})},{path:"/project-definitions/configure",component:Object(p.jsx)(Ct,{})}],badRouteComponent:Object(p.jsx)("div",{children:"Nothing here"})})})}),document.getElementById("root")),h()},56:function(t,e,n){},70:function(t,e,n){}},[[108,1,2]]]);
//# sourceMappingURL=main.77c00c49.chunk.js.map