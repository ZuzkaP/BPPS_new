alter table departments add constraint "FK_are situated" foreign key (location_id)
      references locations (location_id);

alter table departments add constraint "FK_is subsegment of" foreign key (dep_department_id)
      references departments (department_id);
