
function UserProduct(props) {
    let zebra = props.index % 2 == 1 ? " zebra" : "";

    return (
        <div className={"user_product_item " + zebra}>
            <div className="user_product_item_info">
                <a className="user_product_item_name">Наименование: {props.item.name}</a>
                <div className="user_product_item_count">Количество: {props.item.count}</div>
                <div className="user_product_item_price">Цена: {props.item.price}</div>
            </div>
            <div className="user_product_item_interaction">
                <button className="user_product_item_interaction_cancel" onClick={() => props.cancel(props.item.id)}> Убрать </button>
            </div>
        </div>
    );
}

class ProductUserList extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            userProducts: [],
        };
        $.get(window.location.origin + "/api/myProducts", resp => this.setState({ userProducts: resp }));

        this.handleCancellation = this.handleCancellation.bind(this);
    }

    handleCancellation(id) {
        $.post(window.location.origin + "/api/myProduct/cancel", { selectedProductId: id }, resp => {
            this.state.userProducts.splice(this.state.userProducts.findIndex(v => id == v.id), 1)
            this.forceUpdate();
        });
    }

    render() {

        return (
            <div className="product_user_list_wrapper">
                <div className="product_user_list_header">
                    <h2>Ваши товары</h2>
                </div>
                <div className="product_user_list_details">
                    <h3>Количество: {this.state.userProducts.length}</h3>
                </div>
                <div className="product_user_list_body">
                    {this.state.userProducts.map((value, i) => <UserProduct item={value} index={i} id={value.id} key={value.id} cancel={this.handleCancellation} />)}
                </div>
            </div>
        );
    }
}

ReactDOM.render(<ProductUserList />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));