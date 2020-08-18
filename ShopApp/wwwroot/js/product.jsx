
function ProductInfoComment(props) {
    let zebra = props.index % 2 == 1 ? " zebra" : "";
    return (
        <li className={zebra}>
            <a href={window.location.origin + "/profile/" + props.authorNick}>
                {props.authorNick + "   " + props.time}
            </a>
            <p>
                {props.body}
            </p> 
        </li>);
}

class ProductInfoComments extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loadedComments: [],
            pageComment: 0,
            commentBody: "",
            showButton: true,
        };

        this.handlerShowingComments = this.handlerShowingComments.bind(this);
        this.handlerPostingComment = this.handlerPostingComment.bind(this);
    }

    handlerShowingComments() {
        $.get(window.location.origin + "/api/comments/" + this.props.prodId + "/" + this.state.pageComment, resp => {
            this.setState({ pageComment: this.state.pageComment + 1, loadedComments: this.state.loadedComments.concat(resp), showButton: (resp.length < 5) ? false : true });
        });
    }

    handlerPostingComment() {
        $.post(window.location.origin + "/api/add_comment", { commentBody: this.state.commentBody, productId: this.props.prodId }, responseData => {
            if (this.state.showButton)
                this.state.loadedComments.pop();
            this.state.loadedComments.unshift(responseData);
            this.forceUpdate()
        });
    }

    render() {
        return (
            <React.Fragment>
                <div className="product_info_showComments">
                    <div className="product_info_comments_wrapper">
                        <ul>
                            {this.state.loadedComments.map((item, i) => <ProductInfoComment authorNick={item.author.userName} body={item.body} time={item.timePublished} key={i} index={i} />)}
                        </ul>
                    </div>
                    <div className="product_info_button_show_comments">
                        {this.state.showButton ? (<button onClick={this.handlerShowingComments}> Показать больше комментариев </button>) : (<div/>)}
                    </div>
                </div>
                <div className="formForNewComment">
                    <form>
                        <textarea type="text" placeholder="Ваш комментарий" value={this.state.commentBody} onChange={event => this.setState({ commentBody: event.target.value })} />
                        <button type="button" onClick={this.handlerPostingComment}>Отправить</button>
                    </form>
                </div>
            </React.Fragment>
        );
    }
}

class InfoAboutProduct extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            product: null,
            countOfProduct: 1,
        };

        $.get(window.location.origin + "/api" + window.location.pathname, resp => this.setState({ product: resp }));

        this.handlerBuing = this.handlerBuing.bind(this);
    }

    handlerBuing() {
        $.post(window.location.origin + "/api/buy", { productId: this.state.product.id, count: this.state.countOfProduct });
    }

    render() {
        if (this.state.product == null)
            return (<div />);

        return (<div className="product_info">
            <div className="product_info_image">
                <img src={window.location.origin + this.state.product.linkToImage} />
            </div>
            <div className="product_info_basic">
                <div className="product_info_name">
                    <p>{this.state.product.name}</p>
                </div>
                <div className="product_info_basic_space_between">
                    <div className="product_info_price">
                        <p>Цена: {this.state.product.price}</p>
                        <p>Количество: {this.state.product.count}</p>
                    </div>
                    <div className="product_info_author">
                        <a href={window.location.origin + "/profile/" + this.state.product.publisher.userName}>Продавец: {this.state.product.publisher.userName}</a>
                    </div>
                </div>
            </div>
            <div className="product_info_description">
                <p>{this.state.product.description}</p>
            </div>
            <div className="product_info_buyProduct">
                <p>Количество: </p>
                <input type="number" value={this.state.countOfProduct} onChange={e => this.setState({ countOfProduct: e.target.valueAsNumber })} />
                <button onClick={this.handlerBuing}>Купить</button>
            </div>
            <div id="product_info_comments">
                <ProductInfoComments prodId={this.state.product.id} />
            </div>
        </div>);
    }
}


ReactDOM.render(<InfoAboutProduct />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));