alter session set container = XEPDB1;

CREATE USER ADMIN IDENTIFIED BY password;

GRANT CONNECT, RESOURCE TO ADMIN;

create table ADMIN.SAMPLE_MESSAGES (
   id                   number
      generated by default as identity
   primary key,
   message_id           raw(16) default sys_guid() not null,
   message_date         timestamp default current_timestamp not null,
   message_deleted_date timestamp default null
);

create table ADMIN.WO_ACCESS (
   id                   number
      generated by default as identity
   primary key,
   message_id           raw(16) default sys_guid() not null,
   message_date         timestamp default current_timestamp not null,
   message_deleted_date timestamp default null
);

-- Create TESTUSER
CREATE USER TESTUSER IDENTIFIED BY password;

GRANT CONNECT TO TESTUSER;

-- Grant access only to SAMPLE_MESSAGES
GRANT ALL PRIVILEGES ON ADMIN.SAMPLE_MESSAGES TO TESTUSER;

commit;