
class InfoAboutUser extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: {}
        }
        $.get(window.location.origin + "/api" + window.location.pathname, responseData => {
            this.setState({ data: responseData });
            ReactDOM.render(<Orders userName={responseData.nickname} />, document.getElementById("user_orders"));
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
                            Почта подтверждена: {this.state.data.emailConfirmed}
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
                </div>
            </div>
        );
    }
}

function Order(props) {
    let zebra = props.index % 2 == 1 ? " zebra" : "";
    return (<li className={"order_item" + zebra}><a href={window.location.origin + "/order/"+props.orderId}>Заказ номер {props.orderId}</a><p>Дата оформления: {props.dataStart}</p><p>Дата закрытия заказа: {props.dataEnd}</p></li>);
}

class Orders extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            orders: []
        }
        $.get(window.location.origin + "/api/orders/" + props.userName, responseData => this.setState({ orders: responseData }));
    }

    render() {
        let header = this.state.orders.length != 0 ? (<div className="profile_orders_header"><h3>Заказы</h3></div>) : <div />;
        return (
            <React.Fragment>
                {header}
                <ul>{this.state.orders.map((value, index) => <Order key={value.id} orderId={value.id} dataStart={value.dateOfOrdering} dataEnd={value.dateOfClosing} index={index}/>)}</ul>
            </React.Fragment>
        );
    }
}

ReactDOM.render(<InfoAboutUser />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));
