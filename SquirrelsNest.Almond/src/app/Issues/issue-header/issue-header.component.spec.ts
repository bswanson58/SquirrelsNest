import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueHeaderComponent } from './issue-header.component';

describe('IssueHeaderComponent', () => {
  let component: IssueHeaderComponent;
  let fixture: ComponentFixture<IssueHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IssueHeaderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IssueHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
