
class InfoAboutUser extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: {},
            ordersData: (<div/>)
        }
        $.get(window.location.origin + "/api" + window.location.pathname, responseData => {
            this.setState({ data: responseData, ordersData: (<Orders userName={responseData.nickname} />) });
        });
    }

    render() {
        return (
            <div className="profile_wrapper_main">
                <div className="profile_wrapper">
                    <div className="profile_icon">
                        <img href="#" />
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
