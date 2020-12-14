namespace Todo.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Todo.Models;

    public class TodoListViewModel : ViewModelBase
    {
        public TodoListViewModel(IEnumerable<TodoItem> items)
        {
            Items = new ObservableCollection<TodoItem>(items);
        }

        public ObservableCollection<TodoItem> Items { get; }
    }
}