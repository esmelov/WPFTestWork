using Order.Core.Entity;
using Order.Core.Interfaces;
using OrderTestWPF.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using EntityOrder = Order.Core.Entity.Order;

namespace OrderTestWPF.ViewModel
{
    class MainWindowModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _usersObservableCollection;
        private User _currentUser;
        private EntityOrder _currentOrder;
        private IRepository<User, EntityOrder> _userRepo;

        #region Command
        private RelayCommand _addOrder;
        private RelayCommand _deleteOrder;
        private RelayCommand _addUser;
        private RelayCommand _deleteUser;
        private RelayCommand _saveUser;
        private RelayCommand _saveOrder;

        public RelayCommand AddOrder
        {
            get
            {
                return _addOrder ??
                    (_addOrder = new RelayCommand(obj =>
                                    {
                                        if (obj is User curUser)
                                        {
                                            try
                                            {
                                                ObservableCollection<EntityOrder> tmpColl = OrdersObservableCollection;
                                                EntityOrder tmpOrder = new EntityOrder()
                                                {
                                                    UserId = curUser.Id,
                                                    Description = "<Введите описание товара>"
                                                };
                                                tmpColl.Add(tmpOrder);
                                                OrdersObservableCollection = tmpColl;
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                            }
                                            OnPropertyChanged("OrdersObservableCollection");
                                        }
                                    }, obj =>
                                    {
                                        if (obj is User curUser)
                                        {
                                            return (curUser.Id > 0);
                                        }
                                        return false;
                                    }));
            }
        }

        public RelayCommand DeleteOrder
        {
            get
            {
                return _deleteOrder ??
                    (_deleteOrder = new RelayCommand(obj =>
                    {
                        if (obj is EntityOrder curOrder)
                        {
                            try
                            {
                                ObservableCollection<EntityOrder> tmpColl = OrdersObservableCollection;
                                if (curOrder.Id == 0)
                                    tmpColl.Remove(curOrder);
                                else
                                {
                                    Int32 i = _userRepo.Delete(curOrder);
                                    tmpColl.Remove(curOrder);
                                }
                                OrdersObservableCollection = tmpColl;
                                MessageBox.Show($"Товар\r\n\"{curOrder.Description}\"\r\nудалён.", "Информация.", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            OnPropertyChanged("OrdersObservableCollection");
                        }
                    }, obj => obj != null));
            }
        }

        public RelayCommand SaveOrder
        {
            get
            {
                return _saveOrder ??
                    (_saveOrder = new RelayCommand(obj =>
                    {
                        if (obj is EntityOrder curOrder)
                        {
                            try
                            {
                                Int32 i = _userRepo.Save(curOrder);
                                MessageBox.Show($"Товар\r\n\"{curOrder.Description}\"\r\nсохранён.", "Информация.", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            OnPropertyChanged("CurrentOrder");
                        }
                    }, obj =>
                    {
                        if (obj is EntityOrder curOrder)
                        {
                            return !(curOrder.Description == "<Введите описание товара>" || String.IsNullOrWhiteSpace(curOrder.Description));
                        }
                        return false;
                    }));
            }
        }

        public RelayCommand AddUser
        {
            get
            {
                return _addUser ??
                    (_addUser = new RelayCommand(obj =>
                    {
                        try
                        {
                            UsersObservableCollection.Add(new User());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                OnPropertyChanged("UsersObservableCollection");
                    }));
            }
        }

        public RelayCommand DeleteUser
        {
            get
            {
                return _deleteUser ??
                    (_deleteUser = new RelayCommand(obj =>
                    {
                        if (obj is User curUser)
                        {
                            try
                            {
                                if (curUser.Id == 0)
                                    UsersObservableCollection.Remove(curUser);
                                else
                                {
                                    int i = _userRepo.Delete(curUser);
                                    UsersObservableCollection.Remove(curUser);
                                }
                                MessageBox.Show("Пользователь удалён.", "Информация.", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }, obj => obj != null));
            }
        }

        public RelayCommand SaveUser
        {
            get
            {
                return _saveUser ??
                    (_saveUser = new RelayCommand(obj =>
                    {
                        if (obj is User curUsr)
                        {
                            try
                            {
                                Int32 i = _userRepo.Save(curUsr);
                                MessageBox.Show($"Пользователь\r\n{curUsr.Name}\r\nсохранён.", "Информация.", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            OnPropertyChanged("CurrentUser");
                        }
                    }, obj => 
                    {
                        if (obj is User curUser)
                        {
                            return !(String.IsNullOrWhiteSpace(curUser.Name) || String.IsNullOrWhiteSpace(curUser.Adress));
                        }
                        return false;
                    }));
            }
        }
        #endregion

        public ObservableCollection<User> UsersObservableCollection
        {
            get
            {
                return _usersObservableCollection;
            }
            set
            {
                _usersObservableCollection = value;
                OnPropertyChanged("UsersObservableCollection");
            }
        }

        public ObservableCollection<EntityOrder> OrdersObservableCollection
        {
            get
            {
                if (CurrentUser.Orders == null)
                    CurrentUser.Orders = new List<EntityOrder>();
                return new ObservableCollection<EntityOrder>(CurrentUser.Orders);
            }
            set
            {
                CurrentUser.Orders = value;
                OnPropertyChanged("OrdersObservableCollection");
            }
        }

        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
                OnPropertyChanged("OrdersObservableCollection");
            }
        }

        public EntityOrder CurrentOrder
        {
            get
            {
                return _currentOrder;
            }
            set
            {
                _currentOrder = value;
                OnPropertyChanged("CurrentOrder");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public MainWindowModel(IRepository<User, EntityOrder> userRepository)
        {
            try
            {
                _userRepo = userRepository;
                UsersObservableCollection = new ObservableCollection<User>(_userRepo.GetContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(-1);
            }
        }
    }
}
