

/// <reference path="../lib/axios.min.js" />

jQuery.fn.extend({
    getPath: function () {
        var path, node = this;
        while (node.length) {
            var realNode = node[0], name = realNode.localName;
            if (!name) break;
            name = name.toLowerCase();

            var parent = node.parent();

            var sameTagSiblings = parent.children(name);
            if (sameTagSiblings.length > 1) {
                allSiblings = parent.children();
                var index = allSiblings.index(realNode) + 1;
                if (index > 1) {
                    name += ':nth-child(' + index + ')';
                }
            }

            path = name + (path ? '>' + path : '');
            node = parent;
        }

        return path;
    }
});

axios.defaults.baseURL = 'http://localhost:5002';

jsPlumb.ready(function () {
    // setup some defaults for jsPlumb.
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
            ["Label", { label: "FOO", id: "label", cssClass: "aLabel" }]
        ],
        Container: "canvas"
    });

    instance.registerConnectionType("basic", { anchor: "Continuous", connector: "StateMachine" });

    var canvas = document.getElementById("canvas");
    var windows = jsPlumb.getSelector(".statemachine-demo .w");

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
                "cut": { name: "Cut", icon: "cut" },
                "paste": { name: "Paste", icon: "paste" },
                "delete": {
                    name: "Delete", icon: "delete", callback: function (key, opt) {
                        alert("Clicked on " + key + $(opt.$trigger).prev().data('linkId'));
                    }
                },
                "sep1": "---------",
                "quit": {
                    name: "Quit", icon: function () {
                        return 'context-menu-icon context-menu-icon-quit';
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
        info.connection.getOverlay("label").setLabel(info.connection.getData().memo || "");

    });

    // bind a double click listener to "canvas"; add new node when this occurs.
    jsPlumb.on(canvas, "dblclick", function (e) {
        newNode(e.offsetX, e.offsetY);
    });

    //
    // initialise element as connection targets and source.
    //
    var initNode = function (el) {

        // initialise draggable elements.
        instance.draggable(el);

        instance.makeSource(el, {
            filter: ".ep",
            anchor: "Continuous",
            connectorStyle: { stroke: "#5c96bc", strokeWidth: 2, outlineStroke: "transparent", outlineWidth: 4 },
            connectionType: "basic",
            extract: {
                "action": "the-action"
            },
            maxConnections: 2,
            onMaxConnections: function (info, e) {
                alert("Maximum connections (" + info.maxConnections + ") reached");
            }
        });

        instance.makeTarget(el, {
            dropOptions: { hoverClass: "dragHover" },
            anchor: "Continuous",
            allowLoopback: true
        });

        // this is not part of the core demo functionality; it is a means for the Toolkit edition's wrapped
        // version of this demo to find out about new nodes being added.
        //
        instance.fire("jsPlumbDemoNodeAdded", el);
    };

    var newNode = function (x, y) {
        var d = document.createElement("div");
        var id = jsPlumbUtil.uuid();
        d.className = "w";
        d.id = id;
        d.innerHTML = id.substring(0, 7) + "<div class=\"ep\"></div>";
        d.style.left = x + "px";
        d.style.top = y + "px";
        instance.getContainer().appendChild(d);
        initNode(d);
        return d;
    };

    // suspend drawing and initialise.
    instance.batch(function () {
        for (var i = 0; i < windows.length; i++) {
            initNode(windows[i], true);
        }
        // and finally, make a few connections
        instance.connect({ source: "opened", target: "phone1", type: "basic" });
        instance.connect({ source: "phone1", target: "phone1", type: "basic" });
        instance.connect({ source: "phone1", target: "inperson", type: "basic" });

        instance.connect({
            source: "phone2",
            target: "rejected",
            type: "basic"
        });
    });

    jsPlumb.fire("jsPlumbDemoLoaded", instance);



    var container = $(canvas);
    container.html("");
    jsPlumb.reset();
    var jsPlumbWf = {};
    jsPlumbWf.deleteLinks = [];
    jsPlumbWf.deleteTasks = [];
    jsPlumbWf.nodeIndex = 1;

    var drawNode = function (c, node) {
        c.append(("<div type='" + node.type + "' class='w component " + node.class + "' id='" + node.id + "' style='left: " + node.x + "; top: " + node.y + ";" + node.style + " '><div class='taskTitle'>" + node.name + "</div><div class='ep'></div></div>"));
        var $task = $("#" + node.id, c);
        var op = {
            stop: function (event, eui) {
                var oldState = eui.helper.data("State");
                if (oldState == 0) {
                    eui.helper.data("State", 2);
                }
            }
        };
        instance.draggable($task, op);
        instance.makeSource($task, {
            filter: ".ep",
            anchor: "Continuous",
            connectorStyle: { stroke: "#5c96bc", strokeWidth: 2, outlineStroke: "transparent", outlineWidth: 4 },
            connectionType: "basic",
            extract: {
                "action": "the-action"
            },
            maxConnections: 2,
            onMaxConnections: function (info, e) {
                alert("Maximum connections (" + info.maxConnections + ") reached");
            }
        });

        instance.makeTarget($task, {
            dropOptions: { hoverClass: "dragHover" },
            anchor: "Continuous",
            allowLoopback: true
        });

        // this is not part of the core demo functionality; it is a means for the Toolkit edition's wrapped
        // version of this demo to find out about new nodes being added.
        //
        instance.fire("jsPlumbDemoNodeAdded", $task);
        $task.bind("dblclick", function (e) {
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
    };
    var sourceProperty = {
        filter: ".ep", // only supported by jquery
        anchor: "Continuous",
        connector: ["StateMachine", {
            curviness: 30
        }],
        connectorStyle: {
            strokeStyle: "#8b83bc",
            lineWidth: 2,
            outlineColor: "transparent",
            outlineWidth: 4
        },
        maxConnections: 5,
        onMaxConnections: function (info, e) {
            alert("Maximum connections (" + info.maxConnections + ") reached");
        }
    };
    var targetProperty = {
        dropOptions: {
            hoverClass: "dragHover"
        },
        anchor: "Continuous"
    };

    axios.get('/wft/getflowinfo?ID=00001F493WJRC0000A00')
        .then(function (response) {
            if (response.data.success == 0) {
                return;
            }
            var jsonData = response.data.data;

            $.each(jsonData.tasks, function (i, node) {
                drawNode(container, node);
            });
            $.each(jsonData.links, function (i, v) {
                console.log(v.id);
                var q = instance.connect({
                    source: v.begintaskid,
                    target: v.endtaskid,
                    type: "basic",
                    id: v.id,
                    data: v
                });
                $(q.canvas).data('data-info', v);
            });
        })
        .catch(function (error) {
            console.log(error);
        });


    $("#btnGetJson").click(function () {
        var json = {};
        var tmpTasks = [];
        var container = $("#canvas");
        $(".w", container).each(function (i, v) {
            var $task = $(v);
            //if ($task.data("State") == 1 || $task.data("State") == 2) {
            tmpTasks.push({
                state: $task.data("state"),
                id: $task.attr("id"),
                name: $task.text().trim(),
                type: $task.data("type"),
                x: $task.css("left"),
                y: $task.css("top")
            });
            //}
        });
        //$.each(jsPlumbWf.deleteTasks, function (i, v) {
        //    tmpTasks.push({
        //        ID: v,
        //        State: 3
        //    });
        //});
        if (tmpTasks.length > 0)
            json.Tasks = tmpTasks;

        var tmpLinks = [];
        $.each(instance.getAllConnections(), function (i, v) {
            var tmpLink = {
                state: v.state,
                id: v.id,
                begintaskid: v.sourceid,
                endtaskid: v.targetid,
                memo: v.getLabel()
            };
            if (!(tmpLink.State == 0)) {
                tmpLink.State = 1;
                tmpLinks.push(tmpLink);
            }
        });
        //$.each(jsPlumbWf.deleteLinks, function (i, v) {
        //    tmpLinks.push({
        //        ID: v,
        //        State: 3
        //    });
        //});
        if (tmpLinks.length > 0)
            json.Links = tmpLinks;
        alert(JSON.stringify(json));
        axios.post("/wft/updateflowinfo", json).then(function (response) {
            console.log(response);
        })
            .catch(function (error) {
                console.log("error1:" + error);
            });
    });
});
