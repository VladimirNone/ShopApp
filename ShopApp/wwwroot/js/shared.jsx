const Context = React.createContext();

let lastRequest = "";

let hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(window.location.origin + "/notify")
    .build();

hubConnection.on("receive", data => {
    alert(data);
});

//hubConnection.start();

function Loader(props) {
    return (<div style={{ display: 'flex', justifyContent: 'center', margin: '.5rem' }}><div className="lds-dual-ring"></div></div>)
}

function Grid_item(props) {
    return (<li className={"grid__item" + " " + props.additionalClass}>
        <a href={window.location.origin + "/product/" + props.id}>
            <img src={props.image} />
            <div className="item__info">
                <p className="item_name">{props.productName}</p>
                <p className="item_price">{props.productPrice}</p>
            </div>
        </a>
    </li>);

}

function Grid_row(props) {
    return (<li className="grid__row">
        <ul> {props.items.map((item, i) =>
            <Grid_item productName={item.name}
                image={item.linkToImage}
                productPrice={item.price}
                additionalClass={item.additionalClass === undefined ? "" : item.additionalClass}
                key={item.id}
                id={item.id} />)}
        </ul>
    </li>);
};

class Grid extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            countItemsInRow: 4,
            data: [],
            countQuery: 0,
            queryChanged: false,
            loading: true
        };

        this.generateProductList = this.generateProductList.bind(this);
        this.autoLoadProducts = this.autoLoadProducts.bind(this);
        this.callbackFunc = this.callbackFunc.bind(this);
    }

    callbackFunc(newData) {
       
        if (newData.length === 0)
            return;
        if (!this.state.queryChanged)
            newData = this.state.data.concat(newData);

        this.setState({
            data: newData,
            countQuery: this.state.countQuery + 1,
            queryChanged: false,
            loading: false,
        });
    }

    autoLoadProducts() {
        //Текущее расположение пользователя с учетом прокрутки
        let scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        //Высота с учетом прокрутки
        let scrollHeight = Math.max(
            document.body.scrollHeight, document.documentElement.scrollHeight,
            document.body.offsetHeight, document.documentElement.offsetHeight,
            document.body.clientHeight, document.documentElement.clientHeight
        );
        //Высота окна браузера
        let windowHeight = window.innerHeight;

        if (scrollTop + windowHeight > scrollHeight - windowHeight / 2 || this.state.queryChanged) {
            $.get(window.location.origin + "/api" + window.location.pathname + (window.location.pathname.localeCompare("/") == 0 ? "prods/" : "/") + this.state.countQuery, responseData => this.callbackFunc(responseData));
        }
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            countQuery: 0,
            queryChanged: true
        });
    }

    componentWillMount() {
        if (this.props.data != undefined)
            this.setState({ data: this.props.data });
    }

    componentDidMount() {
        this.autoLoadProducts()
        window.setInterval(
            () => this.autoLoadProducts(),
            2000,
        );
    }

    generateProductList() {
        let arr = [];
        let length = this.state.data.length;
        let count = this.state.countItemsInRow;

        for (let i = count; ; i += count) {
            if (i <= length) {
                let buffer = [];
                for (let j = i - count; j < i; j++)
                    buffer.push(this.state.data[j]);
                arr.push((<Grid_row key={i * 1000} items={buffer} />));
            }
            else {
                if (length - (i - count) == 0)
                    break;
                let buffer = [];

                for (let j = 0; j < length - (i - count); j++)
                    buffer.push(this.state.data[(i - count + 1) + j - 1]);

                for (let j = 0; j < count - (length - (i - count)); j++)
                    buffer.push({ linkToImage: "", name: "", price: "", id: -(j + 1), additionalClass: "invisible__product" });

                arr.push((<Grid_row key={i * 1000} items={buffer} />));
                break;
            }
        }
        return arr;
    }

    render() {
        return (
            <div>
                {this.state.loading && <Loader />}
                <ul className="grid">{this.generateProductList()}</ul>
            </div>
        );
    }
}

function Category(props) {
    const { handleClick } = React.useContext(Context);
    return (<li><a onClick={() => handleClick(props.value)}>{props.value}</a></li>);
}

class Categories extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: [],
        };

        $.get(window.location.origin + "/api/categories", resp => this.setState({ data: resp }));

        this.handleClick = this.handleClick.bind(this);
    }

    handleClick(typeOfProduct) {
        window.location.href = window.location.origin + '/category/' + typeOfProduct;
        ReactDOM.render(<Grid />, document.getElementById("content"));
    }

    render() {
        return (
            <Context.Provider value={{ handleClick: this.handleClick }}>
                {this.state.data.map((item, i) => <Category key={item.id} value={item.nameOfType} />)}
            </Context.Provider>
        );
    }
}

class Seacher extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            searchedThing: "",
        }

        this.onChange = this.onChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();
        if (!this.state.searchedThing)
            return;
        window.location.href = window.location.origin + '/search/'+ this.state.searchedThing;
        ReactDOM.render(<Grid  />, document.getElementById("content"));

        this.setState({ searchedThing: "" });
    }

    onChange(e) {
        this.setState({
            searchedThing: e.target.value,
        });
    }

    render() {
        return (<form onSubmit={this.handleSubmit}>
            <input type="text" name="searching_field" placeholder="Поиск" onChange={this.onChange} />
            <button type="submit" formMethod="post">Найти</button>
        </form>);
    }
}

class FormForAuth extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            login: '',
            password: '',
            passwordConfirm: '',
            isOpen: false,
            userAuth: props.authenticated
        }

        this.Authentication = this.Authentication.bind(this);
        this.Authorization = this.Authorization.bind(this);
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
            bindPasswordConfirm: {
                value: this.state.passwordConfirm,
                onChange: event => this.setState({ passwordConfirm: event.target.value })
            },
            clear: () => this.setState({ login: '', password: '', passwordConfirm: ''}),
            login: () => this.state.login,
            password: () => this.state.password,
            passwordConfirm: () => this.state.passwordConfirm,
        }
    }

    Authentication() {
        if (!this.useInputValue().login().trim() || !this.useInputValue().password().trim())
            return;
        $.post(window.location.origin + '/api/account/login', { email: this.useInputValue().login(), password: this.useInputValue().password(), passwordConfirm: this.useInputValue().passwordConfirm() }, responseData => this.state.userAuth());
    }

    Authorization() {
        if (!this.useInputValue().login().trim() || !this.useInputValue().password().trim() || !this.useInputValue().passwordConfirm().trim()) {
            return;
        }
        this.setState({ isOpen: false });
        $.post(window.location.origin + '/api/account/reg', { email: this.useInputValue().login(), password: this.useInputValue().password(), passwordConfirm: this.useInputValue().passwordConfirm() }, responseData => this.state.userAuth());
    } 
     
    render() {
        return (
            <div>
                <form className="auth_form_login">
                    <div className="auth_props">
                        <input name="login" placeholder="Login" {...this.useInputValue().bindLogin} />
                        <input name="password" placeholder="Password" {...this.useInputValue().bindPassword} />
                    </div>
                    <div className="auth_btns">
                        <button type="button" onClick={() => this.Authentication()}>Войти</button>
                        <button type="button" onClick={() => this.setState({ isOpen: true })}>Регистрация</button>
                    </div>
                </form>
                {this.state.isOpen && (
                    <div className='modal'>
                        <div className='modal_body'>
                            <form onSubmit={() => this.Authorization()}>
                                <div className="auth_props">
                                    <label>Введите логин</label>
                                    <input name="login" placeholder="Login" {...this.useInputValue().bindLogin} />
                                    <label>Введите пароль</label>
                                    <input name="password" placeholder="Password" {...this.useInputValue().bindPassword} />
                                    <label>Введите пароль еще раз</label>
                                    <input name="password" placeholder="Confirm your password" {...this.useInputValue().bindPasswordConfirm} />
                                </div>
                                <div className="auth_btns">
                                    <button type="submit">Регистрация</button>
                                    <button onClick={() => this.setState({ isOpen: false })}>Отмена</button>
                                </div>
                            </form>
                        </div>
                    </div>)
                }
            </div>
        );
    }
}

class HandlerUserBlock extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            nickname: "",
            authenticated: false
        };
        $.post(window.location.origin + '/api/account/check', responseData => this.setState({ nickname: responseData.nickname, authenticated: responseData.authenticated }));
    }

    render() {
        return (this.state.authenticated ?
            <UserDataBlock
                exited={() => this.setState({ authenticated: false})}
                userNickName={this.state.nickname} /> :
            <FormForAuth
                authenticated={() => this.setState({ authenticated: true })} />);
    }
}

class UserDataBlock extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            exit: props.exited,
            nickname: props.userNickName
        }
    }

    render() {
        return (
            <div className="user_data_block_wrapper">
                <div className="user_data_block_functional">
                    <ul>
                        <li>Уведомления</li>
                        <li><a href={window.location.origin + "/basket"}>Корзина</a></li>
                        <li><a href={window.location.origin + "/api/account/logout"}>Выйти</a></li>
                    </ul>
                </div>
                <div className="user_data_block_profile">
                    <ul>
                        <li><a href={window.location.origin + "/user_products"}>Мои товары</a></li>
                        <li><a href={window.location.origin + "/new_product"}>Добавить товар</a></li>
                        <li><a href={window.location.origin + "/profile/" + this.state.nickname}>Профиль</a></li>
                    </ul>
                </div>
            </div>
        );
    }
}


