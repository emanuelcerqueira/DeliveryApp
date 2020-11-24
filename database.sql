drop table if exists "delivery";
drop table if exists "location";
drop table if exists "transported_object";
drop table if exists "user";

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

create table "user" (
    id UUID NOT NULL DEFAULT uuid_generate_v1(),
    email character varying(128) not null,
    name character varying(128) not null,
    password character varying(5000) not null,
    telephone character varying(40),
    role character varying(40) not null,
    UNIQUE(email),
    constraint user_pk primary key (id)
);

create table "location" (
    id UUID NOT NULL DEFAULT uuid_generate_v1(),
    latitude DECIMAL(10,6) not null,
    longitude DECIMAL(10,6) not null,
    name character varying(256),
    constraint location_pk primary key (id)
);

create table transported_object(
	id UUID NOT NULL DEFAULT uuid_generate_v1(),
	description character varying(128),
	"height" decimal(4,2) not null,
	"width" decimal(4,2) not null,
	"depth" decimal(4,2) not null,
	"weight" decimal(4,2) not null,
	constraint transported_object_pk primary key (id)
);

create table delivery(
	id UUID NOT NULL DEFAULT uuid_generate_v1(),
	request_date timestamp(4) without time zone not null,
	delivery_date timestamp(4) without time zone,
	customer_id UUID NOT NULL,
	deliveryman_id UUID,
	object_id UUID not null,
	status character varying(40) not null,
	notes character varying(128),
	initial_location_id UUID not null,
	delivery_location_id UUID not null,
	price decimal(10, 2) not null,
	deliveryman_earnings decimal(10, 2) not null,
	distance int,
	constraint delivery_pk primary key (id),
	constraint id_customer_fk foreign key (customer_id) references "user" (id) match simple,
	constraint id_deliveryman_fk foreign key (deliveryman_id) references "user" (id) match simple,
	constraint id_object_fk foreign key (object_id) references transported_object (id) match simple,
	constraint id_initial_location_fk foreign key (initial_location_id) references "location" (id) match simple,
	constraint id_delivery_location_fk foreign key (delivery_location_id) references "location" (id) match simple
);