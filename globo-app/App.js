import 'react-native-gesture-handler';
import { StatusBar } from 'expo-status-bar';
import React from 'react';
import {NavigationContainer} from '@react-navigation/native';
import {createStackNavigator} from '@react-navigation/stack';
import Homepage from './Home';
import {Platform} from 'react-native';
import Header from "./Header";
import { navigationRef } from './RootNavigation';
import Footer from "./Footer";

const Stack = createStackNavigator();

export default function App() {
  return (
      <NavigationContainer
          style={{paddingTop: 0}}
          ref-{navigationRef}
      >
        <Stack.Navigator
            initialRouteName="Globomantics"
            headerMode="screen"
        >
          <Stack.Screen
              name="Globomantics"
              component={Homepage}
              options={{
                  header: () => <Header headerDisplay="Globomantics"/>
              }}
          />
        </Stack.Navigator>
        <Footer/>
      </NavigationContainer>
  );
}


