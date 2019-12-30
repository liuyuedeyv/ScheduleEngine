create table wfapp
(
    id    char(20)     not null
        primary key,
    name  varchar(255) null,
    url   varchar(255) null,
    cuser varchar(8)   not null,
    cdate datetime     not null
)
    charset = utf8;

create table wffins
(
    ID          char(20)     not null
        primary key,
    ServiceID   char(20)     null,
    FLOWID      char(20)     null,
    DATAID      char(20)     not null,
    NAME        varchar(200) not null,
    STATUS      int          not null,
    CTASKID     char(20)     null,
    CTASKNAME   char(20)     null,
    CTASKMEMO   varchar(255) null,
    CUSER       char(8)      null,
    CDATE       datetime     not null,
    EDATE       datetime     null,
    WORKNAME    varchar(255) null,
    MONITOR     varchar(255) null,
    CMUSER      varchar(255) null,
    DEADLINE    datetime     null,
    OFFICALDATE datetime     null,
    LOCKFLAG    int          null,
    TOPFLAG     int          null
)
    charset = utf8;

create table wfflow
(
    ID          char(20)               not null
        primary key,
    serviceid   varchar(20)            not null,
    CUSER       varchar(8)             not null,
    CDATE       datetime               not null,
    version     int         default 1  null,
    released    int         default 0  null,
    releasedate datetime               null,
    istemplate  int         default 1  null,
    baseflowid  varchar(20) default '' null,
    constraint index_wfflow_idversion
        unique (ID, version)
)
    charset = utf8;

create table wflink
(
    ID          char(20)                 not null
        primary key,
    FLOWID      char(20)                 not null,
    BEGINTASKID char(20)                 not null,
    ENDTASKID   char(20)                 not null,
    PRIORITY    int           default 0  not null,
    MEMO        varchar(200)  default '' null,
    LINETYPE    varchar(255)  default '' null,
    FILTER      varchar(1024) default '' null,
    constraint wflink_begin_end
        unique (BEGINTASKID, ENDTASKID),
    constraint fk_wflink_flowid
        foreign key (FLOWID) references wfflow (ID)
)
    charset = utf8;

create index fk_endtaskid_wftaskid
    on wflink (ENDTASKID);

create table wfservice
(
    id            char(20)               not null
        primary key,
    wfappid       varchar(20)            not null,
    name          varchar(255)           null,
    currentflowid varchar(20) default '' not null,
    orderid       int                    null,
    cdate         datetime               not null,
    cuser         varchar(8)             not null,
    datatemplate  varchar(512)           null
)
    charset = utf8;

create table wftask
(
    ID           char(20)                not null
        primary key,
    FLOWID       char(20)                not null,
    NAME         varchar(32)             not null,
    TYPE         int                     not null,
    X            varchar(8)              not null,
    Y            varchar(8)              not null,
    MEMO         varchar(64)  default '' null,
    datatemplate varchar(255) default '' not null,
    setting      varchar(255) default '' not null,
    constraint fk_wftask_flowid
        foreign key (FLOWID) references wfflow (ID)
)
    charset = utf8;

create table wftdata
(
    ID       char(20)     not null
        primary key,
    FLOWID   char(20)     not null,
    FINSID   char(20)     null,
    TASKID   char(20)     not null,
    TINSID   char(20)     null,
    CDATE    datetime     null,
    JSONDATA varchar(255) null
)
    charset = utf8;

create table wftevent
(
    ID           char(20)      not null
        primary key,
    FLOWID       char(20)      not null,
    FINSID       char(20)      not null,
    TASKID       char(20)      not null,
    status       int default 0 not null,
    cdate        datetime      not null,
    processdate  datetime      null,
    dataid       char(20)      null,
    tinsid       varchar(20)   not null,
    waitcallback int default 0 not null,
    callbackdate datetime      null
)
    charset = utf8;

create table wftins
(
    ID        char(20)     not null
        primary key,
    FLOWID    char(20)     not null,
    FINSID    char(20)     null,
    TASKID    char(20)     not null,
    TASKNAME  varchar(200) not null,
    PRETASKID char(20)     null,
    SDATE     datetime     null,
    EDATE     datetime     null,
    MEMO      varchar(255) null,
    DEADLINE  datetime     null
)
    charset = utf8;

