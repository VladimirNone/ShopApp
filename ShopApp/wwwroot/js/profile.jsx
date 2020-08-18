
class InfoAboutUser extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: {},
            ordersData: (<div />),
            hasAccessToRemoveProfile: false,
            isOpen: false,
            login: "",
            password: "",
        }
        $.get(window.location.origin + "/api" + window.location.pathname, responseData => {
            this.setState({ data: responseData, ordersData: (<Orders userName={responseData.nickname} />) });

        }).done(() => {
            $.get(window.location.origin + "/api/canSeeButtonRemove", { username: this.state.data.nickname }, resp => this.setState({ hasAccessToRemoveProfile: resp }));
        });

        this.useInputValue = this.useInputValue.bind(this);
    }

    useInputValue() {
        return {
            bindLogin: {
                value: this.state.login,
                onChange: event => this.setState({ login: event.target.value })
            },
            bindPassword: {
                value: this.state.password,
                onChange: event => this.setState({ password: event.target.value })
            },
            clear: () => this.setState({ login: '', password: '' }),
            login: () => this.state.login,
            password: () => this.state.password,
        }
    }

    removeProfile() {
        this.setState({ isOpen: false });
        $.post(window.location.origin + "/api/account/removeUserAccount", { login: this.state.login, password: this.state.password }, resp => window.location.href = window.location.origin + "/api/account/logout" );
    }

    render() {
        return (
            <div className="profile_wrapper_main">
                <div className="profile_wrapper">
                    <div className="profile_icon">
                        <h3>
                            Место для аватарки
                        </h3>
                    </div>
                    <div className="profile_info">
                        <p>
                            Ник: {this.state.data.nickname}
                        </p>
                        <p>
                            Email: {this.state.data.email}
                        </p>
                        <p>
                            Почта подтверждена: {this.state.data.emailConfirmed == false ? "no" : this.state.data.emailConfirmed}
                        </p>
                        <p>
                            Номер телефона: {this.state.data.phoneNumber == undefined ? "no" : this.state.data.phoneNumber}
                        </p>
                        <p>
                            Дата регистрации: {this.state.data.dateOfRegistration}
                        </p>
                    </div>
                </div>
                {this.state.hasAccessToRemoveProfile ? (
                    <div className="button_remove">
                        <p>Если вы хотите удалить страницу, нажмите на кнопку.</p>
                        <button onClick={() => this.setState({ isOpen: true })}>Удалить страницу</button>
                    </div>
                )
                    : (<div />)}
                {this.state.isOpen && (
                    <div className='modal'>
                        <div className='modal_body modal_body_for_remove'> 
                            <form onSubmit={() => this.removeProfile()}>
                                <div className="auth_props">
                                    <label>Введите логин</label>
                                    <input name="login" placeholder="Login" {...this.useInputValue().bindLogin} />
                                    <label>Введите пароль</label>
                                    <input name="password" placeholder="Password" {...this.useInputValue().bindPassword} />
                                </div>
                                <div className="auth_btns">
                                    <button type="submit">Подтвердить</button>
                                    <button onClick={() => { this.setState({ isOpen: false }) }}>Отмена</button>
                                </div>
                            </form>
                        </div>
                    </div>)
                }
                <div className="profile_orders" id="user_orders">
                    {this.state.ordersData}
                </div>
            </div>
        );
    }
}

function Order(props) {
    let zebra = props.index % 2 == 1 ? " zebra" : "";
    let orderState = ["","Подтвержден","Завершен","Отменен"]
    return (
        <li className={"order_item" + zebra}>
            <div className="functional">
                <a href={window.location.origin + "/order/" + props.value.id}>
                    Заказ номер {props.value.id}
                </a>
                <button onClick={()=>props.cancel(props.index, props.value.id)}>
                    Отменить заказ
                </button>
            </div>
            <p>
                Состояние заказа: {orderState[props.value.state]}
            </p>
            <p>
                Дата оформления: {props.value.dateOfOrdering}
            </p>
            <p>
                Дата оплаты заказа: {props.value.dateOfPaing}
            </p>
            <p>
               Дата закрытия заказа: {props.value.dateOfClosing}
            </p>
        </li>);
}

class Orders extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            orders: []
        }
        $.get(window.location.origin + "/api/orders/" + props.userName, responseData => this.setState({ orders: responseData }));

        this.handlerCancellationOfOrder = this.handlerCancellationOfOrder.bind(this);
    }

    handlerCancellationOfOrder(index, orderId) {
        $.post(window.location.origin + "/api/order/cancel/", { orderId: orderId, username: this.props.userName }, responseData => {
            this.state.orders.splice(index, 1);
            this.forceUpdate();
        })
    }

    render() {
        let header = (this.state.orders.length != 0) ? (<div className="profile_orders_header"><h3>Заказы</h3></div>) : (<div />);
        return (
            <React.Fragment>
                {header}
                <ul>{this.state.orders.map((value, index) => <Order key={value.id} value={value} index={index} cancel={this.handlerCancellationOfOrder}/>)}</ul>
            </React.Fragment>
        );
    }
}

ReactDOM.render(<InfoAboutUser />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));
