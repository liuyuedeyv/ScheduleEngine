/// <reference path="../lib/axios.min.js" />

var EDBEntityState =
{
    UnChanged: 0,
    Added: 2,
    Modified: 4,
    Deleted: 8
};
var ETaskType = {
    Start: 1,
    End: 2,
    Work: 3,
    WorkAsync: 4,
    BingXing: 5,
    JuHe: 6,
    Model: 7
};

axios.defaults.baseURL = document.location.href.replace('5000', '5002');
//axios.defaults.headers = {
//    'Content-type': 'application/x-www-form-urlencoded'
//};
//var flowId = "00001F493WJRC0000A00";
var serviceId = "00001PF1OUJQJ0000A01";
lblServiceId.innerText = serviceId;
var jpdata = {};

function initSetting() {
    var instance = jsPlumb.getInstance({
        Endpoint: ["Dot", { radius: 2 }],
        Connector: "StateMachine",
        HoverPaintStyle: { stroke: "#1e8151", strokeWidth: 2 },
        ConnectionOverlays: [
            ["Arrow", {
                location: 1,
                id: "arrow",
                length: 14,
                foldback: 0.8
            }],
            ["Label", { label: "", id: "label", cssClass: "aLabel" }]
        ],
        Container: "canvas"
    });

    instance.registerConnectionType("basic", { anchor: "Continuous", connector: "StateMachine" });

    var canvas = document.getElementById("canvas");
    // bind a click listener to each connection; the connection is deleted. you could of course
    // just do this: instance.bind("click", instance.deleteConnection), but I wanted to make it clear what was
    // happening.
    //instance.bind("click", function (c) {
    //    instance.deleteConnection(c);
    //});

    instance.bind("contextmenu", function (component, originalEvent) {

        $(component.canvas).data("linkId", $(component.canvas).data('data-info').id)
        $.contextMenu({
            selector: ".aLabel",
            zIndex: 10,
            items: {
                "edit": { name: "Edit", icon: "edit" },
                "delete": {
                    name: "删除规则", icon: "delete", callback: function (key, opt) {
                        var connetionId = $(opt.$trigger).prev().data('linkId');
                        jpdata.deleteLink(connetionId);
                    }
                }
            }
        });
        originalEvent.preventDefault();
        return false;
    });

    // bind a connection listener. note that the parameter passed to this function contains more than
    // just the new connection - see the documentation for a full list of what is included in 'info'.
    // this listener sets the connection's internal
    // id as the label overlay's text.
    instance.bind("connection", function (info) {
        if (info.sourceId == info.targetId) {
            jpdata.deleteLink(info.connection.id);
            return;
        }
        var linkData = info.connection.getData();
        if (linkData.state == undefined) {
            linkData.state = 2;
        }
        info.connection.getOverlay("label").setLabel(linkData.memo || "");
    });
    // bind a double click listener to "canvas"; add new node when this occurs.
    jsPlumb.on("dblclick", function (c) {
        jsPlumbWf.editLink(c.id);
        //newNode(e.offsetX, e.offsetY);
    });
    return instance;
}
jsPlumb.ready(function () {
    // setup some defaults for jsPlumb.
    var instance = initSetting();
    jpdata = {
        instance: instance,
        nodeIndex: 1,
        deleteLinks: [],
        deleteTasks: [],
        getConnectionById: function (linkId) {
            return $.grep(jpdata.instance.getAllConnections(), function (n, i) {
                return n.getData().id == linkId;
            })[0];
        },
        deleteLink: function (linkId) {
            var link = jpdata.getConnectionById(linkId);
            if (link.getData().state == 0) {
                jpdata.deleteLinks.push(linkId);
            }
            jpdata.instance.deleteConnection(link);
        },
        editLink: function (linkId) {
            return;
            $.getFunction("btnSaveClick")(function () {
                var op = {
                    title: "设置流程条件",
                    data: {
                        linkId: linkId,
                        tableCode: $CD.tableCode
                    }
                };
                $.openDialog("wf/WFEditLink", op, function (w, e) {
                    if (e.dialogResult) {
                        if (e.returnValue.LINETYPE == 2) {
                            debug();
                        }
                        var connection = jsPlumbWf.getConnectionById(linkId);
                        connection.getOverlay("label").setLabel("(" + (e.returnValue.PRIORITY || "") + ")" + (e.returnValue.MEMO || ""));
                    }
                });
            });
        },
        deleteTask: function (taskId) {
            //删除任务节点相关线
            var tmpDeleteLinks = [];
            $.each(jpdata.instance.getAllConnections(), function (i, v) {
                if (v.sourceId == taskId || v.targetId == taskId) {
                    tmpDeleteLinks.push(v.getData().id);
                }
            });
            $.each(tmpDeleteLinks, function (i, v) {
                jpdata.deleteLink(v);
            });
            //删除任务节点
            var $task = $("#" + taskId);
            var taskState = $task.data('data').state;
            if (taskState == 0 || taskState == 4) {
                jpdata.deleteTasks.push(taskId);
            }
            $task.remove();
        },
        editTask: function (taskId) {
            alert(taskId);
            return;
            var $task = $("#" + taskId);
            if ($task.data("TYPE") == 1 || $task.data("TYPE") == 2) {
                var op = function (dialogResult, text) {
                    if (dialogResult == false) return;
                    var data = {
                        TableName: "WFTASK",
                        Data: [{ State: 2, ID: taskId, NAME: text }]
                    };
                    $.fn.updateData(data, function (result) {
                        if (result) {
                            $(".taskTitle", $task).text(text);
                        }
                    });
                };
                alertMsg.prompt("请输入任务节点名称", $task.text(), op);
                return;
            }
            $.getFunction("btnSaveClick")(function () {
                $.navigate({
                    "code": "010527",
                    title: "设置任务实例",
                    "data": {
                        taskId: taskId,
                        tableId: $CD.tableId,
                        flowId: $CD.flowId
                    }
                });
            });
        },
        clear: function (container) {
            jpdata.instance.clear();
            jpdata.instance.reset();
            jpdata.instance = initSetting();
            jpdata.deleteLinks = [];
            jpdata.deleteTasks = [];
            jpdata.nodeIndex = 1;
            container.html('');
        },
        initNodeData: function (node) {
            if (node.type == 1)//开始节点
            {
                if (node.name == undefined) {
                    node.name = "开始节点";
                }
            }
            else if (node.type == 2)//结束节点
            {
                if (node.name == undefined) {
                    node.name = "结束节点";
                }
            }
            else {
                if (node.name == undefined) {
                    node.name = "节点" + jpdata.nodeindex++;
                }
                node.name = node.name;
            }
            node.class = "wtype" + node.type;
            return node;
        },
        getOneId: async function () {
            try {
                return await axios.get("/wft/getid");
            }
            catch (e) {
                console.log(e);
            }
        },
        addNode: function (container, node) {
            this.drawNode(container, node).data("state", 2);
        },
        drawNode: function (c, node) {
            node = this.initNodeData(node);
            c.append(("<div type='" + node.type + "' data-shape='Diamond'  class='w component " + node.class + "' id='" + node.id + "' style='left: " + node.x + "; top: " + node.y + ";" + node.style + " '><div class='taskTitle'>" + node.name + "</div><div class='ep'></div></div>"));
            var $task = $("#" + node.id, c);
            $task.data('data', node);

            var op = {
                stop: function (e) {
                    var oldData = $(e.el).data("data");
                    if (oldData.state == 0) {
                        oldData.state = 4;
                    }
                }
            };
            try {
                jpdata.instance.draggable($task, op);
            }
            catch (e) {
                console.log(e);
            }
            if (node.type != 2) {
                jpdata.instance.makeSource($task, {
                    filter: ".ep",
                    anchor: "Continuous",
                    connectorStyle: { stroke: "#5c96bc", strokeWidth: 2, outlineStroke: "transparent", outlineWidth: 4 },
                    connectionType: "basic",
                    extract: {
                        "action": "the-action"
                    },
                    maxConnections: 5,
                    onMaxConnections: function (info, e) {
                        alert("Maximum connections (" + info.maxConnections + ") reached");
                    }
                });
            }
            else {
                $(".ep", $task).remove();

            }
            if (node.type != 1) {
                jpdata.instance.makeTarget($task, {
                    dropOptions: { hoverClass: "dragHover" },
                    anchor: "Continuous",
                    allowLoopback: false
                });
            }
            // this is not part of the core demo functionality; it is a means for the Toolkit edition's wrapped
            // version of this demo to find out about new nodes being added.
            //
            jpdata.instance.fire("jsPlumbDemoNodeAdded", $task);


            $.contextMenu({
                selector: "#" + node.id,
                zIndex: 10,
                items: {
                    "edit": {
                        name: "修改名称", icon: "edit", callback: function (key, opt) {
                            var $node = $(opt.$trigger);
                            var $title = $(".taskTitle", $node);
                            var result = window.prompt("请输入新的名称", $title.text());
                            if (result != null) {
                                $title.text(result);
                                $node.data('data').name = result;
                                $node.data('data').state = 4;
                            }
                        }
                    },
                    "delete": {
                        name: "删除", icon: "delete", callback: function (key, opt) {
                            if (window.confirm('确定要删除吗？')) {
                                jpdata.deleteTask($(opt.$trigger).attr('id'));
                            }
                            return true;
                        }
                    }
                }
            });

            $task.bind("dblclick", function (e) {

                var result = window.prompt("请输入新的名称", $(e.target).text());
                if (result != null) {
                    $(e.target).text(result);
                    $(e.currentTarget).data('data').name = result;
                    $(e.currentTarget).data('data').state = 4;
                }
                return;
                alert('dbclick');
                var id = e.target.id;
                if (id == "") {
                    id = $(e.target).parent().attr("id");
                }
                if (isApproved) {
                    alertMsg.confirm("此节点处理人相关信息已经生成，您确定要修改吗？", function (isModify) {
                        if (isModify) {
                            jsPlumbWf.editTask(id);
                        }
                    });
                }
                else {
                    jsPlumbWf.editTask(id);
                }
            });
            return $task;
        }
    };
});


function loadFlowInfo(flowId) {
    var container = $(canvas);
    jpdata.clear(container);
    container.attr('flowiId', flowId);
    axios.get('/wft/getflowinfo?ID=' + flowId)
        .then(function (response) {
            if (response.data.success == 0) {
                return;
            }
            var jsonData = response.data.data;

            $.each(jsonData.tasks, function (i, node) {
                jpdata.drawNode(container, node);
            });
            $.each(jsonData.links, function (i, v) {
                var q = jpdata.instance.connect({
                    source: v.begintaskid,
                    target: v.endtaskid,
                    type: "basic",
                    id: v.id,
                    data: v
                });
                $(q.canvas).data('data-info', v);
            });
            jpdata.instance.fire("jsPlumbDemoLoaded", jpdata.instance);

        })
        .catch(function (error) {
            console.log(error);
        });
}

axios.get('/wft/getallflows?serviceId=' + serviceId)
    .then(function (response) {
        if (response.data.success == 0) {
            return;
        }
        var jsonData = response.data.data;
        var $c = $("#flowlist");
        $.each(jsonData, function (i, v) {
            $c.append('<li> <input flowid="' + v.id + '" type="button" value="V' + v.version + '【' + (v.released ? "已发布" : "未发布") + '】 ' + (v.currentTag ? '【当前】' : '') + ' " /></li >');
        });

        $("input[type='button']", $c).click(function () {

            $("li", $("#flowlist")).removeClass("select");
            $(this).parent().addClass("select");
            loadFlowInfo($(this).attr('flowid'));
        });
    })
    .catch(function (error) {
        console.log(error);
    });


$("#btnGetJson").click(function () {
    var container = $("#canvas");
    var json = {
        "id": container.attr('flowiId')
    };
    var tmpTasks = [];
    $(".w", container).each(function (i, v) {
        var $task = $(v);
        var taskData = $task.data("data");
        if (taskData.state == 2 || taskData.state == 4) {
            tmpTasks.push({
                state: taskData.state,
                id: taskData.id,
                name: $task.text().trim(),
                type: taskData.type,
                x: $task.css("left"),
                y: $task.css("top")
            });
        }
    });
    $.each(jpdata.deleteTasks, function (i, v) {
        tmpTasks.push({
            ID: v,
            state: 8
        });
    });
    if (tmpTasks.length > 0)
        json.Tasks = tmpTasks;

    var tmpLinks = [];
    $.each(jpdata.instance.getAllConnections(), function (i, v) {
        var linkData = v.getData();
        var tmpLink = {
            state: linkData.state,
            id: linkData.id,
            begintaskid: v.sourceId,
            endtaskid: v.targetId,
            memo: v.getLabel()
        };
        if (!(tmpLink.state == 0)) {
            tmpLink.state = 2;
            tmpLinks.push(tmpLink);
        }
    });
    $.each(jpdata.deleteLinks, function (i, v) {
        tmpLinks.push({
            ID: v,
            State: 8
        });
    });
    if (tmpLinks.length > 0)
        json.Links = tmpLinks;
    axios.post("/wft/updateflowinfo", json).then(function (response) {
        console.log(response);
        alert('保存成功！');
    })
        .catch(function (error) {
            console.log("error1:" + error);
        });
});
var x = 0;
var y = 0;
$("#btnAddNode").click(function () {
    axios.get("/wft/getid").then(function (res) {
        x += 5;
        y += 5;
        if (x % 20 == 0) {
            x = y = 10;
        }
        var newId = res.data.data;
        jpdata.addNode($("#canvas"), {
            id: newId,
            x: x + "px",
            y: y + "px",
            type: 3,
            state: 2
        });
    });
});


$("#btnRelease").click(function () {
    var data = {
        flowId: $("#flowlist>.select input").attr('flowid')
    };
    var selectFlowid = $("#flowlist>.select input").attr('flowid');
    axios.post("/wft/releaseflow", 'flowId=' + data.flowId).then(function (res) {
        window.location.href = window.location.href;
    });
});
$("#btnSetCurrent").click(function () {
    var data = {
        flowId: $("#flowlist>.select input").attr('flowid'),
        serviceId: serviceId
    };
    axios.post("/wft/setCurrent", 'serviceId=' + data.serviceId + '&flowId=' + data.flowId).then(function (res) {
        window.location.href = window.location.href;
    });
});