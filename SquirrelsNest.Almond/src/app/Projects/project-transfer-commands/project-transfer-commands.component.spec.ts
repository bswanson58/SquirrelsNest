import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTransferCommandsComponent } from './project-transfer-commands.component';

describe('ProjectTransferCommandsComponent', () => {
  let component: ProjectTransferCommandsComponent;
  let fixture: ComponentFixture<ProjectTransferCommandsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectTransferCommandsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectTransferCommandsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
