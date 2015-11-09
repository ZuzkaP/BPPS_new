alter table departments add constraint "FK_are situated" foreign key (location_id)
      references locations (location_id);
