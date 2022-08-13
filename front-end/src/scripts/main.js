import UIkit from './uikit/api/index';
import boot from './uikit/api/boot';
import * as components from './components';
import {each} from 'uikit-util';

// register components
each(components, (component, name) => UIkit.component(name, component));

boot(UIkit);

window.UIkit = UIkit;