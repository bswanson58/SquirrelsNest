import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectImportDialogComponent } from './project-import-dialog.component';

describe('ProjectImportDialogComponent', () => {
  let component: ProjectImportDialogComponent;
  let fixture: ComponentFixture<ProjectImportDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectImportDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectImportDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
