import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueTypesListComponent } from './issue-types-list.component';

describe('IssueTypesListComponent', () => {
  let component: IssueTypesListComponent;
  let fixture: ComponentFixture<IssueTypesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IssueTypesListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IssueTypesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
