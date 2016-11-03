/*
Joney.
*/
(function($) {
   var defaults = {
      processData:{},//步骤节点数据
      //processUrl:'',//步骤节点数据
      fnRepeat:function(){
        layer.alert("步骤连接重复");
      },
      fnClick:function(){
          layer.alert("单击");
      },
      fnDbClick:function(){
          layer.alert("双击");
      },
      canvasMenus : {
          "one": function (t) { layer.alert('画面右键') }
      },
      processMenus: {
          "one": function (t) { layer.alert('步骤右键') }
      },
      /*右键菜单样式*/
      menuStyle: {
        border: '1px solid #5a6377',
        minWidth:'150px',
        padding:'5px 0'
      },
      itemStyle: {
        fontFamily : 'verdana',
        color: '#333',
        border: '0',
        /*borderLeft:'5px solid #fff',*/
        padding:'5px 40px 5px 20px'
      },
      itemHoverStyle: {
        border: '0',
        /*borderLeft:'5px solid #49afcd',*/
        color: '#fff',
        backgroundColor: '#5a6377'
      },
      mtAfterDrop:function(params){
          //alert('连接成功后调用');
          //alert("连接："+params.sourceId +" -> "+ params.targetId);
      },
      //这是连接线路的绘画样式
      connectorPaintStyle : {
          lineWidth:1,
          strokeStyle: "#808080",
          joinstyle: "round",
          outlineColor: "#808080",
          outlineWidth:1
      },
      //鼠标经过样式
      connectorHoverStyle : {
          lineWidth:1,
          strokeStyle: "red",
          outlineColor:"red",
          outlineWidth:1
      }

   };/*defaults end*/

    // this is the paint style for the connecting lines..
   var connectorPaintStyle = {
       lineWidth:1,
       strokeStyle: "#808080",
       joinstyle: "round",
       outlineColor: "#808080",
       outlineWidth:1
   },
   connectorPaintStyle_Red = {
       lineWidth:1,
       strokeStyle: "red",
       joinstyle: "round",
       outlineColor: "red",
       outlineWidth:1
   },
   // .. and this is the hover style.
       connectorHoverStyle = {
           lineWidth:1,
           strokeStyle: "red",
           outlineWidth:1,
           outlineColor: "red"
       },
       endpointHoverStyle = {
           fillStyle: "red",
           strokeStyle: "red"
       },
   // the definition of source endpoints (the small blue ones)
       sourceEndpoint = {
           endpoint: "Dot",
           paintStyle: {
               strokeStyle: "#808080",
               fillStyle: "transparent",
               radius:2,
               lineWidth:2
           },
           isSource: true,
           connector: ["Flowchart", { stub: [40, 60], gap: 10, cornerRadius: 5, alwaysRespectStubs: true }],
           connectorStyle: connectorPaintStyle,
           hoverPaintStyle: endpointHoverStyle,
           connectorHoverStyle: connectorHoverStyle,
           dragOptions: {},
           overlays: [
               ["Label", {
                   location: [0.5, 1.5],
                   label: "Drag",
                   cssClass: "endpointSourceLabel",
                   visible: false
               }]
           ]
       },
   // the definition of target endpoints (will appear when the user drags a connection)
       targetEndpoint = {
           endpoint: "Dot",
           paintStyle: { fillStyle: "#808080", radius: 11 },
           hoverPaintStyle: endpointHoverStyle,
           maxConnections: -1,
           dropOptions: { hoverClass: "hover", activeClass: "active" },
           isTarget: true,
           overlays: [
               ["Label", { location: [0.5, -0.5], label: "Drop", cssClass: "endpointTargetLabel", visible: false }]
           ]
       };
   //实例化线条
   var initEndPoints = function(){
      $(".process-flag").each(function(i,e) {
          var p = $(e).parent();
          var conPaintStyle = connectorPaintStyle;//connectorPaintStyle_Red,connectorPaintStyle
          jsPlumb.makeSource($(e), {
              parent:p,
              anchor:"Continuous",
              endpoint:[ "Dot", { radius:1} ],
              connector:[ "Flowchart", { stub:[5, 5] } ],
              connectorStyle:defaults.connectorPaintStyle,//defaults.connectorPaintStyle,connectorPaintStyle
              hoverPaintStyle:defaults.connectorHoverStyle,
              dragOptions:{},
              maxConnections:-1
          });
      });
   }
   $("._jsPlumb_overlay.aLabel").live("mouseover", function (e) {
       //console.log(e);
       //layer.tips(e.target.innerText, this);
   });
    //注册线标双击事件
   $("._jsPlumb_overlay.aLabel").live("dblclick", function (e) {
       var curElem = $(this);
       $("#selActionBox #txt_labelTitle").val(curElem[0].innerText.split('-')[0]);
       $("#selActionBox #selElem").val(curElem[0].innerText.split('-')[1]);
       layer.open({
           type: 1,
           title: '流程条件',
           shadeClose: false,
           area: ['300px', '200px'],
           content: $("#selActionBox"),
           btn: ['确定', '取消'],
           yes: function (e) {
               var time = $("#txt_labelTitle").val();
               var ac = $("#selElem").find("option:selected").text();
               str = time + "-" + ac;
               curElem.attr("title", str);
               curElem.attr("data-value", str);
               curElem[0].innerText = str;
               layer.close(e);
           },
           cancel: function (i) {
               console.log("No." + i);
               layer.close(i);
           }
       });
   });
  //实例化线条标签
   var initLabel = function (connection) {
       var sourceText = connection.source[0].innerText;//.attributes.Title.value; //connection.source.innerText;//connection.sourceId.substring(15)
       var targetText = connection.target[0].innerText;//.attributes.Title.value;// connection.target.innerText;//connection.targetId.substring(15)
       connection.getOverlay("label").setLabel(sourceText + "-" + targetText);
       
   };
   
  /*设置隐藏域保存关系信息*/
  var aConnections = [];
  var setConnections = function (conn, remove) {
      if (conn.sourceId === conn.targetId) {
          return false;
      }
      if (!remove) aConnections.push(conn);
      else {
          var idx = -1;
          for (var i = 0; i < aConnections.length; i++) {
              if (aConnections[i] == conn) {
                  idx = i; break;
              }
          }
          if (idx != -1) aConnections.splice(idx, 1);
      }
      if (aConnections.length > 0) {
          var s = "";
          for ( var j = 0; j < aConnections.length; j++ ) {
              var from = $('#'+aConnections[j].sourceId).attr('process_id');
              var target = $('#'+aConnections[j].targetId).attr('process_id');
              s = s + "<input type='hidden' id=\"lb_"+from+"_"+target+"\" value=\"" + from + "," + target + "\" data-value=\""+from+","+target+"\">";
          }
          $('#leipi_process_info').html(s);
      } else {
          $('#leipi_process_info').html('');
      }
      jsPlumb.repaintEverything();//重画
  };

   /*Flowdesign 命名纯粹为了美观，而不是 formDesign */
   var FD=$.fn.Flowdesign = function(options)
    {
        var _canvas = $(this);
        //右键步骤的步骤号
        _canvas.append('<input type="hidden" id="leipi_active_id" value="0"/><input type="hidden" id="leipi_copy_id" value="0"/>');
        _canvas.append('<div id="leipi_process_info"></div>');

        /*配置*/
        $.each(options, function(i, val) {
          if (typeof val == 'object' && defaults[i])
            $.extend(defaults[i], val);
          else 
            defaults[i] = val;
        });
        /*画布右键绑定*/
        var contextmenu = {
          bindings: defaults.canvasMenus,
          menuStyle : defaults.menuStyle,
          itemStyle : defaults.itemStyle,
          itemHoverStyle : defaults.itemHoverStyle
        }
        $(this).contextMenu('canvasMenu',contextmenu);

        jsPlumb.importDefaults({
            DragOptions: { cursor: 'pointer' },//拖动时鼠标停留在该元素上显示指针，通过css控制
            EndpointStyle : { fillStyle:'#225588' },//连接点默认颜色
            Endpoint : [ "Dot", {radius:2} ],//连接点的默认形状
            ConnectionOverlays : [
                [ "Arrow", { location:1 } ],
                [ "Label", {
                    location:0.1,
                    id:"label",
                    cssClass:"aLabel"
                }]
            ],
            Anchor : 'Continuous',//连接点位置
            ConnectorZIndex:5,
            HoverPaintStyle:defaults.connectorHoverStyle
        });
       //默认连线样式：
        //var basicType = {
        //    //connector: "StateMachine",
        //    connector: ["Flowchart",
        //        { stub: [40, 60], gap: 10, cornerRadius: 5, alwaysRespectStubs: true }
        //    ],
        //    paintStyle: { strokeStyle: "red", lineWidth:3 },
        //    hoverPaintStyle: { strokeStyle: "blue" },
        //    overlays: [
        //        "Arrow"
        //    ]
        //};
        //jsPlumb.registerConnectionType("basic", basicType);//初始化连线样式

        if( $.browser.msie && $.browser.version < '9.0' ){ //ie9以下，用VML画图
            jsPlumb.setRenderMode(jsPlumb.VML);
        } else { //其他浏览器用SVG
            jsPlumb.setRenderMode(jsPlumb.SVG);
        }

    //初始化原步骤
    var lastProcessId=0;
    var processData = defaults.processData;
    if(processData.list)
    {
        $.each(processData.list, function(i,row) 
        {
            var nodeDiv = document.createElement('div');
            var nodeId = "window_" + row.id, badge = 'badge-inverse',icon = 'icon-star';
            if(lastProcessId==0)//第一步
            {
              badge = 'badge-info';
              icon = 'icon-play';
            }
            if(row.icon)
            {
              icon = row.icon;
            }
            $(nodeDiv).attr("id",nodeId)
            .attr("style",row.style)
            .attr("process_to",row.process_to)
            .attr("process_id", row.id)
            .attr("nod_title", row.process_name)
            .addClass("process-step btn btn-small "+row.node_class+"")
            .html('<span class="process-flag badge '+badge+'"><i class="'+icon+' icon-white"></i></span>&nbsp;<em>' + row.process_name+'</em>')
            .mousedown(function(e){
              if( e.which == 3 ) { //右键绑定
                  _canvas.find('#leipi_active_id').val(row.id);
                  contextmenu.bindings = defaults.processMenus
                  $(this).contextMenu('processMenu', contextmenu);
              }
            });
            _canvas.append(nodeDiv);
            //索引变量
            lastProcessId = row.id;
        });//each
    }

    var timeout = null;
    //点击或双击事件,这里进行了一个单击事件延迟，因为同时绑定了双击事件
    $(".process-step").live('click',function(){
        //激活
        _canvas.find('#leipi_active_id').val($(this).attr("process_id")),
        clearTimeout(timeout);
        var obj = this;
        timeout = setTimeout(defaults.fnClick,300);
    }).live('dblclick',function(){
        clearTimeout(timeout);
        defaults.fnDbClick();
    });

    //使之可拖动
    jsPlumb.draggable(jsPlumb.getSelector(".process-step"));
    initEndPoints();

    //绑定添加连接操作。画线-input text值  拒绝重复连接
    jsPlumb.bind("jsPlumbConnection", function(info) {
        setConnections(info.connection)
    });
    //连线标签
    jsPlumb.bind("connection", function (connInfo, originalEvent) {
        initLabel(connInfo.connection);
    });
    //绑定删除connection事件
    jsPlumb.bind("jsPlumbConnectionDetached", function(info) {
        setConnections(info.connection, true);
    });
    //绑定删除确认操作
    jsPlumb.bind("dblclick", function (c) {
        console.log(c);

        layer.confirm('你确定删除吗?', function (index) {
            jsPlumb.detach(c);
            layer.close(index);
        });
      //if(confirm("你确定取消连接吗?"))
        
    });
    var targetElement = null;
    //单击连线设置获取属性
    jsPlumb.bind("click", function (c) {
        //if (confirm("你确定取消连接吗?"))
        //jsPlumb.detach(c);
        targetElement = c;
    });
    //连接成功回调函数
    function mtAfterDrop(params)
    {
        //console.log(params)
        defaults.mtAfterDrop({sourceId:$("#"+params.sourceId).attr('process_id'),targetId:$("#"+params.targetId).attr('process_id')});
    }
    
    jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
        dropOptions:{ hoverClass:"hover", activeClass:"active" },
        anchor:"Continuous",
        maxConnections:-1,
        endpoint: ["Dot", { radius: 1}],
        paintStyle: { fillStyle: "#ec912a", radius: 2 },
        hoverPaintStyle:this.connectorHoverStyle,
        beforeDrop: function (params) {
            console.log(params);
            if(params.sourceId == params.targetId) return false;/*不能链接自己*/
            var j = 0;
            $('#leipi_process_info').find('input').each(function(i){
                var str = $('#' + params.sourceId).attr('process_id') + ',' + $('#' + params.targetId).attr('process_id');
                if(str == $(this).val()){
                    j++;
                    return;
                }
            })
            if( j > 0 ){
                defaults.fnRepeat();
                return false;
            } else {
                mtAfterDrop(params);
                return true;
            }
        }
    });
    //reset  start
    var _canvas_design = function(){
        //连接关联的步骤
        $('.process-step').each(function(i){
            var sourceId = $(this).attr('process_id');
            //var nodeId = "window"+id;
            var prcsto = $(this).attr('process_to');
            var toArr = prcsto.split(",");
            var processData = defaults.processData;
            $.each(toArr,function(j,targetId){
                
                if(targetId!='' && targetId!=0){
                    //检查 source 和 target是否存在
                    var is_source = false,is_target = false;
                    $.each(processData.list, function(i,row) 
                    {
                        if(row.id == sourceId)
                        {
                            is_source = true;
                        }else if(row.id == targetId)
                        {
                            is_target = true;
                        }
                        if(is_source && is_target)
                            return true;
                    });

                    if(is_source && is_target)
                    {
                        jsPlumb.connect({
                            source:"window_"+sourceId, 
                            target:"window_"+targetId
                            ,labelStyle : { cssClass:"component label" }
                            //,label :" id + - "+ n
                        });
                        return ;
                    }
                }
            })
        });
    }//_canvas_design end reset 
    _canvas_design();

//-----外部调用----------------------

    var Flowdesign = {
        
        addProcess:function(row){
            
                if(row.id<=0)
                {
                    return false;
                }
                var nodeDiv = document.createElement('div');
                var nodeId = "window_" + row.id, badge = 'badge-inverse',icon = 'icon-star';
                
                if(row.icon)
                {
                    icon = row.icon;
                }
                $(nodeDiv).attr("id",nodeId)
                .attr("style",row.style)
                .attr("process_to",row.process_to)
                .attr("process_id", row.id)
                .attr("nod_title", row.process_name)
                .addClass("process-step btn btn-small "+row.node_class+"")
                .html('<span class="process-flag badge ' + badge + '"><i class="' + icon + ' icon-white"></i></span>&nbsp;<em>' + row.process_name + '</em></div>')
                .mousedown(function(e){
                  if( e.which == 3 ) { //右键绑定
                      _canvas.find('#leipi_active_id').val(row.id);
                      contextmenu.bindings = defaults.processMenus
                      $(this).contextMenu('processMenu', contextmenu);
                  }
                });
                
                _canvas.append(nodeDiv);
                //使之可拖动 和 连线
                jsPlumb.draggable(jsPlumb.getSelector(".process-step"));
                initEndPoints();
                //使可以连接线
                jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
                    dropOptions:{ hoverClass:"hover", activeClass:"active" },
                    anchor:"Continuous",
                    maxConnections:-1,
                    endpoint:[ "Dot", { radius:1 } ],
                    paintStyle:{ fillStyle:"#ec912a",radius:1 },
                    hoverPaintStyle:this.connectorHoverStyle,
                    beforeDrop:function(params){
                        var j = 0;
                        $('#leipi_process_info').find('input').each(function (i) {
                            var str = $('#' + params.sourceId).attr('process_id') + ',' + $('#' + params.targetId).attr('process_id');
                            if (str == $(this).val()) {
                                j++;
                                return;
                            }
                        });
                        if( j > 0 ){
                            defaults.fnRepeat();
                            return false;
                        } else {
                            return true;
                        }
                    }
                });
                return true;
                
        },
        delProcess:function(activeId){
            if(activeId<=0) return false;
            
            $("#window_"+activeId).remove();
            return true;
        },
        getActiveId:function()
        {
          return _canvas.find("#leipi_active_id").val();
        },
        copy:function(active_id){
        if(!active_id)
          active_id = _canvas.find("#leipi_active_id").val();

        _canvas.find("#leipi_copy_id").val(active_id);
        return true;
        },
        paste:function(){
            return  _canvas.find("#leipi_copy_id").val();
        },
        getProcessInfo:function()
        {
            try{
              /*连接关系*/
              var aProcessData = {};
              $("#leipi_process_info input[type=hidden]").each(function(i){
                  var processVal = $(this).val().split(",");
                  if(processVal.length==2)
                  {
                    if(!aProcessData[processVal[0]])
                    {
                        aProcessData[processVal[0]] = {"top":0,"left":0,"process_to":[]};
                    }
                    aProcessData[processVal[0]]["process_to"].push(processVal[1]);
                  }
              })
              /*位置*/
              _canvas.find("div.process-step").each(function(i){ //生成Json字符串，发送到服务器解析
                      if($(this).attr('id')){
                          var pId = $(this).attr('process_id');
                          var pLeft = parseInt($(this).css('left'));
                          var pTop = parseInt($(this).css('top'));
                         if(!aProcessData[pId])
                          {
                              aProcessData[pId] = {"top":0,"left":0,"process_to":[]};
                          }
                          aProcessData[pId]["top"] =pTop;
                          aProcessData[pId]["left"] =pLeft;

                      }
                  })
             return JSON.stringify(aProcessData);
          }catch(e){
              return '';
            }
        },
        getProcessData: function () {//Debug by: Joney.
            try {
                /*连接关系*/
                var aProcessData = {};
                var total = 0;
                var list = {};
                var flowID = the_flow_id;

                $("#leipi_process_info input[type=hidden]").each(function (i) {
                    console.log($(this));
                    var processVal = $(this).val().split(",");
                    if (processVal.length == 2) {
                        if (!aProcessData[processVal[0]]) {
                            aProcessData[processVal[0]] = { "top": 0, "left": 0, "process_to": [] };
                        }
                        aProcessData[processVal[0]]["process_to"].push(processVal[1]);
                        
                    }
                });

                /*位置*/
                _canvas.find("div.process-step").each(function (e) { //生成Json字符串，发送到服务器解析
                    console.log($(this));
                    total++;
                    if ($(this).attr('id')) {
                        var pId = $(this).attr('process_id');
                        var pTitle=$(this).attr('nod_title');
                        var pLeft = parseInt($(this).css('left'));
                        var pTop = parseInt($(this).css('top'));
                        if (!aProcessData[pId]) {
                            aProcessData[pId] = { "top": 0, "left": 0, "process_to": [] };
                        }
                        var nodeClass = $(this).find("i")[0].className.split(' ')[0];
                        var thisClass = $(this)[0].className.split(' ');
                        var curClass = "";
                        for (var i = 0; i < thisClass.length; i++) {
                            if (thisClass[i].split('-')[0]=='nod') {
                                curClass = thisClass[i]+" ";
                            }
                        }
                        aProcessData[pId]["id"] = pId;
                        aProcessData[pId]["flow_id"] = flowID;
                        aProcessData[pId]["process_name"] = pTitle;
                        aProcessData[pId]["process_to"] = aProcessData[pId]["process_to"].join(',');
                        aProcessData[pId]["icon"] = nodeClass;
                        aProcessData[pId]["node_class"] = curClass;
                        aProcessData[pId]["style"] = "left:" + pLeft + "px;top:" + pTop + "px;";
                        aProcessData[pId]["top"] = pTop;
                        aProcessData[pId]["left"] = pLeft;
                    }
                });
                var listJson = new Array();
                for (var item in aProcessData) {
                    listJson.push(
                    {
                        "id": aProcessData[item]["id"],
                        "flow_id": aProcessData[item]["flow_id"],
                        "process_name": aProcessData[item]["process_name"],
                        "process_to": aProcessData[item]["process_to"],
                        "icon": aProcessData[item]["icon"],
                        "node_class": aProcessData[item]["node_class"],
                        "style": aProcessData[item]["style"]
                    });
                }
                var pData = { "total": total, "list": listJson };
                return JSON.stringify(pData);
            } catch (e) {
                return '';
            }
        },
        clear:function()
        {
            try{

                jsPlumb.detachEveryConnection();
                jsPlumb.deleteEveryEndpoint();
                $('#leipi_process_info').html('');
                jsPlumb.repaintEverything();
                return true;
            }catch(e){
                return false;
            }
        },
        refresh: function ()
        {
            try{
                //jsPlumb.reset();
                this.clear();
                _canvas_design();
                return true;
            }catch(e){
                return false;
            }
        }
    };
    return Flowdesign;
  }//$.fn

})(jQuery);