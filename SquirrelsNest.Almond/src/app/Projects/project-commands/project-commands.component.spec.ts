import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCommandsComponent } from './project-commands.component';

describe('ProjectCommandsComponent', () => {
  let component: ProjectCommandsComponent;
  let fixture: ComponentFixture<ProjectCommandsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectCommandsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectCommandsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
