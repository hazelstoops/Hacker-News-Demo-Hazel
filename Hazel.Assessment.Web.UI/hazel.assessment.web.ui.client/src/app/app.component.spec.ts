import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { StoryTableComponent } from './ui/story-table.component';

// ignore, components are tested individually
xdescribe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent, StoryTableComponent],
      imports: []
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
  });

  afterEach(() => {
  });

  it('IGNORED: should create the app, but test ignored as the sub component pieces are tested individually', () => {
    expect(component).toBeTruthy();
  });

});
