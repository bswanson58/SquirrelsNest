import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectUsersListComponent } from './project-users-list.component';

describe('ProjectUsersListComponent', () => {
  let component: ProjectUsersListComponent;
  let fixture: ComponentFixture<ProjectUsersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectUsersListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectUsersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
